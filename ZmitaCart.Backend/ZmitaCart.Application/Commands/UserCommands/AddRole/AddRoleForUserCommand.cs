using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.UserCommands.AddRole;

public record AddRoleForUserCommand : IRequest<Result>
{
	public string? UserEmail { get; init; }
	public string? Role { get; init; }
}