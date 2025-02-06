using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.UserCommands.DeleteFeedback;

public record DeleteFeedbackCommand(int Id) : IRequest<Result>;