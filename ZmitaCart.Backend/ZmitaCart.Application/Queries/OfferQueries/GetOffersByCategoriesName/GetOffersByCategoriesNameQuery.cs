using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;

namespace ZmitaCart.Application.Queries.OfferQueries.GetOffersByCategoriesName;

public record GetOffersByCategoriesNameQuery(List<string> CategoriesNames, int Size) : IRequest<Result<List<NamedList<string, OfferInfoDto>>>>;