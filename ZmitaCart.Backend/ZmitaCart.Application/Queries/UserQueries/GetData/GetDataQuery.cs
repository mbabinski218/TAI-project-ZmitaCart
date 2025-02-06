using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.UserDtos;

namespace ZmitaCart.Application.Queries.UserQueries.GetData;

public record GetDataQuery() : IRequest<Result<UserDataDto>>;