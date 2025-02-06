using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.UserDtos;

namespace ZmitaCart.Application.Commands.UserCommands.LoginUser;

public record LoginUserCommand : IRequest<Result<TokensDto>>
{
	public string GrantType { get; init; } = null!;
	public string? Email { get; init; }
	public string? Password { get; init; }
	public string? RefreshToken { get; init; }
	public string? IdToken { get; init; }
}