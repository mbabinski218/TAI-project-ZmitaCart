using FluentResults;
using MediatR;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Commands.UserCommands.DeleteFeedback;

public class DeleteFeedbackHandler : IRequestHandler<DeleteFeedbackCommand, Result>
{
	private readonly IUserRepository _userRepository;

	public DeleteFeedbackHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<Result> Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
	{
		return await _userRepository.DeleteFeedbackAsync(request.Id);
	}
}