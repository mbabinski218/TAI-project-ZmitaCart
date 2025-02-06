using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;

namespace ZmitaCart.Application.Queries.OfferQueries.SearchOffers;

public record SearchOffersQuery : IRequest<Result<PaginatedList<OfferInfoDto>>>
{
	public string? Title { get; init; }
	public int? CategoryId { get; init; }
	public IEnumerable<string>? Conditions { get; init; }
	public decimal? MinPrice { get; init; }
	public decimal? MaxPrice { get; init; }
	public string? SortBy { get; init; }
	public int? PageNumber { get; init; }
	public int? PageSize { get; init; }
}