using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Behaviors;

public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest> 
	where TRequest : notnull
{
	private readonly ILogger<LoggingBehavior<TRequest>> _logger;
	private readonly ICurrentUserService _currentUserService;

	public LoggingBehavior(ILogger<LoggingBehavior<TRequest>> logger, ICurrentUserService currentUserService)
	{
		_logger = logger;
		_currentUserService = currentUserService;
	}

	public Task Process(TRequest request, CancellationToken cancellationToken)
	{
		var user = _currentUserService.UserId ?? "Anonymous";
		
		_logger.LogInformation("Request: {User} {@Request}", user, request);
		
		return Task.CompletedTask;
	}
}