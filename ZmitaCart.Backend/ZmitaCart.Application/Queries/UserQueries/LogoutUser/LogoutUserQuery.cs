using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Queries.UserQueries.LogoutUser;

public record LogoutUserQuery : IRequest<Result>;