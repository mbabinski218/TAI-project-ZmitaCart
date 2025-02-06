using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ZmitaCart.Application.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
	where TRequest : notnull
	where TResponse : notnull
{
	private readonly Stopwatch _timer;
	private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

	public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
	{
		_timer = new Stopwatch();
		_logger = logger;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		_timer.Start();

		var response = await next();

		_timer.Stop();

		var elapsedTicks = _timer.ElapsedTicks;

		_logger.LogInformation("Time: {ElapsedTicks} ticks", elapsedTicks);

		return response;
	}
}