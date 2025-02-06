using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.OfferDtos;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOfferData;

public record GetOfferDataQuery(int Id) : IRequest<Result<OfferDataDto>>;