using System.Security.Claims;
using FluentResults;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Domain.Common;
using ZmitaCart.Domain.Common.Types;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Common.Settings;
using ZmitaCart.Infrastructure.Services;

namespace ZmitaCart.Infrastructure.Common;

public class GoogleAuthentication
{
	private readonly UserManager<User> _userManager;
	private readonly JwtHelper _jwtHelper;
	private readonly GoogleSettings _googleSettings;
	private readonly IUserEventLoggerService _userEventLoggerService;

	public GoogleAuthentication(IOptions<GoogleSettings> googleSettings, UserManager<User> userManager, 
		JwtHelper jwtHelper, IUserEventLoggerService userEventLoggerService)
	{
		_userManager = userManager;
		_jwtHelper = jwtHelper;
		_userEventLoggerService = userEventLoggerService;
		_googleSettings = googleSettings.Value;
	}

	public async Task<Result<TokensDto>> AuthenticateAsync(string idToken)
	{
		var payload = await VerifyTokenAsync(idToken);
		if (payload.IsFailed)
		{
			await _userEventLoggerService.LogUserLoggedInFailureAsync("Google - Failed to authenticate user - idToken: " + idToken, null);
			return payload.ToResult<TokensDto>();
		}
		
		var user = await _userManager.FindByEmailAsync(payload.Value.Email);
		if (user == null)
		{
			var register = await RegisterAsync(payload.Value);
			if (register.IsFailed)
			{
				await _userEventLoggerService.LogUserRegisteredFailureAsync("Google - Failed to register user - idToken: " + idToken, payload.Value.Email);
				return register.ToResult<TokensDto>();
			}
		}
		
		return await LoginAsync(payload.Value);
	}

	private async Task<Result<GoogleJsonWebSignature.Payload>> VerifyTokenAsync(string token)
	{
		var settings = new GoogleJsonWebSignature.ValidationSettings
		{
			Audience = new List<string>{ _googleSettings.ClientId },
		};
		
		var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
		if (payload == null) return Result.Fail(new InvalidDataError("Invalid Google token"));

		return payload;
	}
	
	private async Task<Result> RegisterAsync(GoogleJsonWebSignature.Payload payload)
	{
		var user = new User
		{
			Email = payload.Email,
			UserName = payload.Email,
			FirstName = payload.GivenName,
			LastName = payload.FamilyName,
			EmailConfirmed = true
		};

		var result = await _userManager.CreateAsync(user);

		if (!result.Succeeded)
		{
			var reasons = result.Errors.Select(e => e.Description);
			return Result.Fail(new InvalidDataError("Failed to register", reasons));
		}

		await _userManager.AddToRoleAsync(user, Role.user);

		var claims = new List<Claim>
		{
			new(ClaimNames.Id, user.Id.ToString()),
			new(ClaimNames.Email, user.Email),
			new(ClaimNames.FirstName, user.FirstName),
			new(ClaimNames.LastName, user.LastName),
			new(ClaimNames.Role, Role.user)
		};
		
		await _userManager.AddClaimsAsync(user, claims);
		await _userManager.AddLoginAsync(user, new UserLoginInfo("Google", payload.Subject, "Google"));
		
		return Result.Ok();
	}
	
	private async Task<Result<TokensDto>> LoginAsync(GoogleJsonWebSignature.Payload payload)
	{
		var user = await _userManager.FindByEmailAsync(payload.Email);
		if (user == null)
		{
			await _userEventLoggerService.LogUserLoggedInFailureAsync("Google - Failed to authenticate user", payload.Email);
			return Result.Fail(new InvalidDataError("Failed to authenticate user"));
		}

		var authClaims = await _userManager.GetClaimsAsync(user);

		var accessToken = _jwtHelper.GenerateAccessToken(authClaims);
		var refreshToken = _jwtHelper.GenerateRefreshToken();
		
		await RemoveTokens(user);
		await _userManager.SetAuthenticationTokenAsync(user, GrantType.google, "RefreshToken", refreshToken);

		await _userManager.ResetAccessFailedCountAsync(user);
		await _userEventLoggerService.LogUserLoggedInSuccessAsync("Google - User logged in", user.Id, user.Email!);
		return new TokensDto
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken
		};
	}
	
	private async Task RemoveTokens(User user)
	{
		foreach (var authenticator in GrantType.SupportedGrantTypes)
		{
			await _userManager.RemoveAuthenticationTokenAsync(user, authenticator, "RefreshToken");
		}
	}
}