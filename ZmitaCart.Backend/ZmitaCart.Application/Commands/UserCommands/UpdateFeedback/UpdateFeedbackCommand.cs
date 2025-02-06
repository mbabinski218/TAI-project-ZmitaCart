using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.UserCommands.UpdateFeedback;

public record UpdateFeedbackCommand : IRequest<Result<int>>
{
	public int  Id { get; set; }
	public int? Rating { get; set; }
	public string? Comment { get; set; }
}