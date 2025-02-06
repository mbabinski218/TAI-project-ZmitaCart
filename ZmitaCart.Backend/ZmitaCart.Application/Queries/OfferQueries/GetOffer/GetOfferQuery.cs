using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.OfferDtos;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOffer;

public record GetOfferQuery(int Id) : IRequest<Result<OfferDto>>;