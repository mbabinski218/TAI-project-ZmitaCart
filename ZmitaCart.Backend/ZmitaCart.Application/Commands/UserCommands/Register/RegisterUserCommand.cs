using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.UserCommands.Register;

public record RegisterUserCommand : IRequest<Result>
{
	public string Email { get; init; } = null!;
	public string FirstName { get; init; } = null!;
	public string LastName { get; init; } = null!;
	public string Password { get; init; } = null!;
	public string ConfirmedPassword { get; init; } = null!;
	public bool? IsAdmin { get; init; }
}