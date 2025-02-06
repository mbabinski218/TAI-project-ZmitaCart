using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.UserCommands.ResendEmailConfirmation;

public sealed record ResendEmailConfirmationCommand(string Email) : IRequest<Result>;