using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;

namespace ZmitaCart.Application.Queries.OfferQueries.GetBoughtOffers;

public record GetBoughtOffersQuery : IRequest<Result<PaginatedList<BoughtOfferDto>>>
{
	public int? PageNumber { get; init; }
	public int? PageSize { get; init; }
}