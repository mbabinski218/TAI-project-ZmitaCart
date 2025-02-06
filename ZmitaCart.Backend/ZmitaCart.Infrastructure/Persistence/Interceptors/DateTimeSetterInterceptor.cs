using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Infrastructure.Persistence.Interceptors;

public sealed class DateTimeSetterInterceptor : SaveChangesInterceptor
{
	private readonly ILogger<DateTimeSetterInterceptor> _logger;

	public DateTimeSetterInterceptor(ILogger<DateTimeSetterInterceptor> logger)
	{
		_logger = logger;
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		_logger.LogDebug("Setting DateTime...");
		
		DateTimeSetter(eventData.Context);
		return base.SavingChanges(eventData, result);
	}
	
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new())
	{
		_logger.LogDebug("Setting DateTime...");
		
		DateTimeSetter(eventData.Context);
		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private static void DateTimeSetter(DbContext? context)
	{
		context?.ChangeTracker
			.Entries()
			.Where(entry => entry.State is EntityState.Added or EntityState.Modified)
			.ToList()
			.ForEach(entry =>
			{
				if (entry.Entity is not IEntity entity)
				{
					return;
				}

				switch (entry.State)
				{
					case EntityState.Added:
						entity.CreatedAt = DateTimeOffset.UtcNow;
						break;
					case EntityState.Modified:
						entity.UpdatedAt = DateTimeOffset.UtcNow;
						break;
				}
			});
	}
}