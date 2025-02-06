using FluentResults;
using MediatR;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Commands.UserCommands.UpdateFeedback;

public class UpdateFeedbackHandler : IRequestHandler<UpdateFeedbackCommand, Result<int>>
{
	private readonly IUserRepository _userRepository;
	private readonly ICurrentUserService _currentUserService;

	public UpdateFeedbackHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
	{
		_userRepository = userRepository;
		_currentUserService = currentUserService;
	}

	public async Task<Result<int>> Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			return Result.Fail(new UnauthorizedError("You must be logged in to update feedback"));
		}

		var raterId = int.Parse(_currentUserService.UserId);
		
		return await _userRepository.UpdateFeedbackAsync(request.Id, raterId, request.Rating, request.Comment);
	}
}