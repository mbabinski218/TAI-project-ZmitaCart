using FluentResults;
using MediatR;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Commands.UserCommands.GiveFeedback;

public class GiveFeedbackHandler : IRequestHandler<GiveFeedbackCommand, Result<int>>
{
	private readonly IUserRepository _userRepository;
	private readonly ICurrentUserService _currentUserService;

	public GiveFeedbackHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
	{
		_userRepository = userRepository;
		_currentUserService = currentUserService;
	}

	public async Task<Result<int>> Handle(GiveFeedbackCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			return Result.Fail(new UnauthorizedError("You must be logged in to give feedback"));
		}
		
		var raterId = int.Parse(_currentUserService.UserId);
		
		return await _userRepository.GiveFeedbackAsync(raterId, request.RecipientId, request.Rating, request.Comment);
	}
}