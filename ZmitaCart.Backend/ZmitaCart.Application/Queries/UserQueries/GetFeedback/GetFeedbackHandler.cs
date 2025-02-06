using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Queries.UserQueries.GetFeedback;

public class GetFeedbackHandler : IRequestHandler<GetFeedbackQuery, Result<PaginatedList<FeedbackDto>>>
{
	private readonly IUserRepository _userRepository;

	public GetFeedbackHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<Result<PaginatedList<FeedbackDto>>> Handle(GetFeedbackQuery request, CancellationToken cancellationToken)
	{
		return await _userRepository.GetFeedbackAsync(request.UserId ,request.PageNumber, request.PageSize);
	}
}