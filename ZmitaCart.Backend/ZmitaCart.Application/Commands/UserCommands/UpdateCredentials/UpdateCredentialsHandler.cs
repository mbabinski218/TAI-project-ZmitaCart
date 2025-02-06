using FluentResults;
using MediatR;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Commands.UserCommands.UpdateCredentials;

public class UpdateCredentialsHandler : IRequestHandler<UpdateCredentialsCommand, Result>
{
	private readonly IUserRepository _userRepository;
	private readonly ICurrentUserService _currentUserService;

	public UpdateCredentialsHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
	{
		_userRepository = userRepository;
		_currentUserService = currentUserService;
	}

	public async Task<Result> Handle(UpdateCredentialsCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId == null)
		{
			return Result.Fail(new UnauthorizedError("Log in to use this function"));
		}
		
		return await _userRepository.UpdateCredentialsAsync(_currentUserService.UserId, request.PhoneNumber, request.Address);
	}
}