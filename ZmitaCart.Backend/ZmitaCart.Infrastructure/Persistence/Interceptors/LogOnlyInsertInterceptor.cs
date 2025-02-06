using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Infrastructure.Persistence.Interceptors;

public class LogOnlyInsertInterceptor : SaveChangesInterceptor
{
	private const string errorMessage = "Update and delete operations are not allowed on entities implementing ILogEntity";
	
	private readonly ILogger<LogOnlyInsertInterceptor> _logger;

	public LogOnlyInsertInterceptor(ILogger<LogOnlyInsertInterceptor> logger)
	{
		_logger = logger;
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		_logger.LogDebug("Checking if it's insert...");

		if (!CheckIfOnlyInserts(eventData))
		{
			_logger.LogError(errorMessage);
			throw new InvalidOperationException(errorMessage);
		}
		
		return base.SavingChanges(eventData, result);
	}

	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
		InterceptionResult<int> result, CancellationToken cancellationToken = new())
	{
		_logger.LogDebug("Checking if it's insert...");

		if (!CheckIfOnlyInserts(eventData))
		{
			_logger.LogError(errorMessage);
			throw new InvalidOperationException(errorMessage);
		}
		
		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}
	
	private static bool CheckIfOnlyInserts(DbContextEventData eventData)
	{
		return eventData.Context != null 
		       && !eventData.Context.ChangeTracker
			       .Entries()
			       .Where(e => e.Entity is ILogEntity)
			       .Any(e => e.State is EntityState.Modified or EntityState.Deleted);
	}
}