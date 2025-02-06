using System.Security.Claims;
using FluentResults;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.LogDtos;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Domain.Common;
using ZmitaCart.Domain.Common.Models;
using ZmitaCart.Domain.Common.Types;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Domain.ValueObjects;
using ZmitaCart.Infrastructure.Common;
using ZmitaCart.Infrastructure.Persistence.DbContexts;
using ZmitaCart.Infrastructure.Services;

namespace ZmitaCart.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
	private readonly UserManager<User> _userManager;
	private readonly RoleManager<IdentityUserRole> _roleManager;
	private readonly JwtHelper _jwtHelper;
	private readonly ApplicationDbContext _dbContext;
	private readonly GoogleAuthentication _googleAuthentication;
	private readonly IUserEventLoggerService _userEventLoggerService;

	public UserRepository(UserManager<User> userManager, RoleManager<IdentityUserRole> roleManager, JwtHelper jwtHelper,
		ApplicationDbContext dbContext, GoogleAuthentication googleAuthentication, IUserEventLoggerService userEventLoggerService)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_jwtHelper = jwtHelper;
		_dbContext = dbContext;
		_googleAuthentication = googleAuthentication;
		_userEventLoggerService = userEventLoggerService;
	}

	public async Task<Result<UserDataDto>> GetDataAsync(int id)
	{
		var user = await _dbContext.Users
			.FirstOrDefaultAsync(u => u.Id == id);

		return user is null 
			? Result.Fail<UserDataDto>(new NotFoundError("User not found")) 
			: user.Adapt<UserDataDto>();
	}

	public async Task<Result<User>> RegisterAsync(RegisterUserDto dto)
	{
		if (await _userManager.FindByEmailAsync(dto.Email) != null)
		{
			await _userEventLoggerService.LogUserRegisteredFailureAsync("User already exists", dto.Email);
			return Result.Fail(new AlreadyExistsError("User already exists"));
		}

		var user = User.Create(dto.Email, dto.FirstName, dto.LastName);
		var result = await _userManager.CreateAsync(user, dto.Password);

		if (!result.Succeeded)
		{
			var reasons = result.Errors.Select(e => e.Description).ToList();
			await _userEventLoggerService.LogUserRegisteredFailureAsync("Invalid register data: " + string.Concat(reasons), dto.Email);
			return Result.Fail(new InvalidDataError("Invalid register data", reasons));
		}
		
		var role = dto.IsAdmin is true ? Role.administrator : Role.user;
		await _userManager.AddToRoleAsync(user, role);
		
		if (role is Role.administrator)
			await _userManager.AddToRoleAsync(user, Role.user);

		var claims = new List<Claim>
		{
			new(ClaimNames.Id, user.Id.ToString()),
			new(ClaimNames.Email, user.Email!),
			new(ClaimNames.FirstName, user.FirstName),
			new(ClaimNames.LastName, user.LastName),
			new(ClaimNames.Role, role)
		};
		await _userManager.AddClaimsAsync(user, claims);

		await _userEventLoggerService.LogUserRegisteredSuccessAsync("User registered successfully", user.Id, user.Email!);
		return user;
	}

	public async Task<Result<TokensDto>> LoginAsync(LoginUserDto loginUserDto)
	{
		var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
		if (user == null)
		{
			await _userEventLoggerService.LogUserLoggedInFailureAsync("User does not exist", loginUserDto.Email);
			return Result.Fail(new NotFoundError("User does not exist"));
		}

		if (!await _userManager.IsEmailConfirmedAsync(user))
		{
			await _userManager.AccessFailedAsync(user);
			await _userEventLoggerService.LogUserLoggedInFailureAsync("Email is not confirmed", loginUserDto.Email);
			return Result.Fail(new InvalidDataError("Email is not confirmed"));
		}

		if (!await _userManager.CheckPasswordAsync(user, loginUserDto.Password))
		{
			await _userManager.AccessFailedAsync(user);
			await _userEventLoggerService.LogUserLoggedInFailureAsync("Invalid password", loginUserDto.Email);
			return Result.Fail(new InvalidDataError("Invalid password"));
		}

		var authClaims = await _userManager.GetClaimsAsync(user);
		var accessToken = _jwtHelper.GenerateAccessToken(authClaims);
		var refreshToken = _jwtHelper.GenerateRefreshToken();

		await RemoveTokens(user);
		await _userManager.SetAuthenticationTokenAsync(user, GrantType.password, "RefreshToken", refreshToken);
		
		await _userManager.ResetAccessFailedCountAsync(user);
		await _userEventLoggerService.LogUserLoggedInSuccessAsync("User logged in successfully", user.Id, user.Email!);
		return new TokensDto
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken
		};
	}

	public async Task<Result<TokensDto>> LoginWithRefreshTokenAsync(string refreshToken)
	{
		var userToken = await _dbContext.UserTokens.FirstOrDefaultAsync(t => t.Value == refreshToken);

		if (userToken is null)
		{
			await _userEventLoggerService.LogUserLoggedInFailureAsync("Try to login with invalid refresh token: " + refreshToken, null);
			return Result.Fail(new NotFoundError("User does not exist"));
		}
		
		var user = await _userManager.FindByIdAsync(userToken.UserId.ToString());

		if (user is null)
		{
			await _userEventLoggerService.LogUserLoggedInFailureAsync("Invalid id in refresh token: " + refreshToken, null);
			return Result.Fail(new NotFoundError("User does not exist"));
		}
		
		foreach(var authenticator in GrantType.SupportedGrantTypes)
		{
			var userRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, authenticator, "RefreshToken");

			if (userRefreshToken is null)
			{
				continue;
			}
			
			if (userRefreshToken != refreshToken)
			{
				await _userManager.RemoveAuthenticationTokenAsync(user, authenticator, "RefreshToken");
				await _userManager.AccessFailedAsync(user);
				await _userEventLoggerService.LogUserLoggedInFailureAsync("Outdated or invalid refresh token: " + refreshToken, user.Email!);
				return Result.Fail(new InvalidDataError("Invalid refresh token"));
			}

			var authClaims = await _userManager.GetClaimsAsync(user);
			var accessToken = _jwtHelper.GenerateAccessToken(authClaims);
			var newRefreshToken = _jwtHelper.GenerateRefreshToken();

			await RemoveTokens(user);
			await _userManager.SetAuthenticationTokenAsync(user, GrantType.refreshToken, "RefreshToken", newRefreshToken);

			await _userManager.ResetAccessFailedCountAsync(user);
			await _userEventLoggerService.LogUserLoggedInSuccessAsync("User logged in successfully with refresh token", user.Id, user.Email!);
			return new TokensDto
			{
				AccessToken = accessToken,
				RefreshToken = newRefreshToken
			};
		}
		
		await _userManager.AccessFailedAsync(user);
		await _userEventLoggerService.LogUserLoggedInFailureAsync("Failed to login with refresh token (invalid grant type): " + refreshToken, user.Email!);
		return Result.Fail(new InvalidDataError("Failed to login with refresh token"));
	}

	public async Task<Result<TokensDto>> LoginWithGoogleAsync(string idToken)
	{
		return await _googleAuthentication.AuthenticateAsync(idToken);
	}

	public async Task<Result> LogoutAsync(int userId)
	{
		var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

		if (user is null)
		{
			await _userEventLoggerService.LogUserLoggedOutFailureAsync("User does not exist", userId);
			return Result.Fail(new NotFoundError("User does not exist"));
		}
		await RemoveTokens(user);
		
		await _userEventLoggerService.LogUserLoggedOutSuccessAsync("User logged out successfully", user.Id, user.Email!);
		return Result.Ok();
	}

	public async Task<Result> AddRoleAsync(string userEmail, string role)
	{
		var user = await _userManager.FindByEmailAsync(userEmail);
		
		if (user == null)
			return Result.Fail(new NotFoundError("User does not exist"));

		if (!await _roleManager.RoleExistsAsync(role))
			return Result.Fail(new NotFoundError("Role does not exist"));
		
		if (!await _userManager.IsInRoleAsync(user, role))
			return Result.Fail(new InvalidDataError("User already has this role"));

		await _userManager.AddToRoleAsync(user, role);
		await _userManager.AddClaimAsync(user, new Claim(ClaimNames.Role, role));

		return Result.Ok();
	}

	public async Task<Result> UpdateCredentialsAsync(string userId, string? phoneNumber, Address? address)
	{
		var user = await _userManager.FindByIdAsync(userId);

		if (user is null)
		{
			return Result.Fail(new NotFoundError("User does not exist"));
		}

		if (phoneNumber is not null)
		{
			var result = await _userManager.SetPhoneNumberAsync(user, phoneNumber);

			if (!result.Succeeded)
			{
				return Result.Fail(new InvalidDataError("Could not set phone number"));
			}
		}

		if (address is not null && address != user.Address)
		{
			user.Address = address;

			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded)
			{
				return Result.Fail(new InvalidDataError("Could not set address"));
			}
		}
		
		return Result.Ok();
	}

	public async Task<Result<int>> GiveFeedbackAsync(int raterId, int recipientId, int rating, string? comment)
	{
		var rater = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == raterId);

		if (rater is null)
		{
			return Result.Fail(new NotFoundError("Rater does not exist"));
		}

		var feedback = new Feedback
		{
			RecipientId = recipientId,
			RaterId = raterId,
			Rater = rater,
			Rating = rating,
			Comment = comment
		};

		await _dbContext.Feedbacks.AddAsync(feedback);
		await _dbContext.SaveChangesAsync();
		return Result.Ok(feedback.Id);
	}

	public async Task<Result<int>> UpdateFeedbackAsync(int feedbackId, int raterId, int? rating, string? comment)
	{
		var feedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(f => f.Id == feedbackId);

		if (feedback is null)
		{
			return Result.Fail(new NotFoundError("Feedback does not exist"));
		}
			
		if (feedback.RaterId != raterId)
		{
			return Result.Fail(new UnauthorizedError("You are not allowed to update this feedback"));
		}
		
		feedback.Rating = rating ?? feedback.Rating;
		feedback.Comment = comment ?? feedback.Comment;

		await _dbContext.SaveChangesAsync();
		return Result.Ok(feedback.Id);
	}

	public async Task<Result> DeleteFeedbackAsync(int feedbackId)
	{
		var feedback = await _dbContext.Feedbacks.FirstOrDefaultAsync(f => f.Id == feedbackId);

		if (feedback is null)
		{
			return Result.Fail(new NotFoundError("Feedback does not exist"));
		}
			
		_dbContext.Feedbacks.Remove(feedback);
		await _dbContext.SaveChangesAsync();
		return Result.Ok();
	}

	public async Task<Result<PaginatedList<FeedbackDto>>> GetFeedbackAsync(int userId, int? pageNumber, int? pageSize)
	{
		var feedback = await _dbContext.Feedbacks
			.Where(f => f.RecipientId == userId)
			.Include(f => f.Rater)
			.ProjectToType<FeedbackDto>()
			.ToPaginatedListAsync(pageNumber, pageSize);
		
		return Result.Ok(feedback);
	}
	
	public async Task<Result<User>> FindByIdAsync(int id)
	{
		var user = await _dbContext.Users.FindAsync(id);
		return user is null 
			? Result.Fail<User>(new NotFoundError("User not found")) 
			: Result.Ok(user);
	}
	
	public async Task<Result<User>> FindByEmailAsync(string email)
	{
		var user = await _userManager.FindByEmailAsync(email);
		return user is null 
			? Result.Fail<User>(new NotFoundError("User not found")) 
			: Result.Ok(user);
	}
	  
	public async Task<Result> ConfirmEmailAsync(User user, string token)
	{
		var result = await _userManager.ConfirmEmailAsync(user, token);
		return !result.Succeeded 
			? Result.Fail(new InvalidDataError("Email confirmation failed")) 
			: Result.Ok();
	}
	
	public async Task<Result<string>> GenerateEmailConfirmationTokenAsync(User user)
	{
		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		return Result.Ok(token);
	}
	
	private async Task RemoveTokens(User user)
	{
		foreach (var authenticator in GrantType.SupportedGrantTypes)
		{
			await _userManager.RemoveAuthenticationTokenAsync(user, authenticator, "RefreshToken");
		}
	}
	
	public async Task<Result<PaginatedList<LogDto>>> GetLogsAsync(string? searchText, bool? isSuccess,
		DateTimeOffset? from, DateTimeOffset? to, int? pageNumber, int? pageSize)
	{
		return await _userEventLoggerService.GetLogsAsync(searchText, isSuccess, from, to, pageNumber, pageSize);
	}
}