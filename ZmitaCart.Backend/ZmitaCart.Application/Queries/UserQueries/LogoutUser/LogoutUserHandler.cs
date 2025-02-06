using FluentResults;
using MediatR;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Queries.UserQueries.LogoutUser;

public class LogoutUserHandler : IRequestHandler<LogoutUserQuery, Result>
{
	private readonly IUserRepository _userRepository;
	private readonly ICurrentUserService _currentUserService;

	public LogoutUserHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
	{
		_userRepository = userRepository;
		_currentUserService = currentUserService;
	}

	public async Task<Result> Handle(LogoutUserQuery request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			return Result.Fail(new UnauthorizedError("You are not authorized."));
		}
		
		var userId = int.Parse(_currentUserService.UserId);
		return await _userRepository.LogoutAsync(userId);
	}
}