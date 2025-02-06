using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Queries.OfferQueries.GetFavouritesOffersCount;

public record GetFavouritesOfferCountQuery() : IRequest<Result<int>>;