using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.UserCommands.ConfirmEmail;

public sealed record ConfirmEmailCommand(
	int Id, 
	string Token) 
	: IRequest<Result>;