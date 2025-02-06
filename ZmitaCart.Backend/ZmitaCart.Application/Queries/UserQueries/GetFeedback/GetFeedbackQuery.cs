using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.UserDtos;

namespace ZmitaCart.Application.Queries.UserQueries.GetFeedback;

public record GetFeedbackQuery : IRequest<Result<PaginatedList<FeedbackDto>>>
{
	public int UserId { get; init; }
	public int? PageNumber { get; init; }
	public int? PageSize { get; init; }
}