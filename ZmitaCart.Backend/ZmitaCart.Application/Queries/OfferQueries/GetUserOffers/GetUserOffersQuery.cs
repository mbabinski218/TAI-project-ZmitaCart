using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;

namespace ZmitaCart.Application.Queries.OfferQueries.GetUserOffers;

public record GetUserOffersQuery : IRequest<Result<PaginatedList<OfferInfoDto>>>
{
	public int? PageNumber { get; init; }
	public int? PageSize { get; init; }
}