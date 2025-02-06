using Microsoft.EntityFrameworkCore;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Persistence.Interceptors;

namespace ZmitaCart.Infrastructure.Persistence.DbContexts;

public sealed class LogDbContext : DbContext
{
	// DbSet
	public DbSet<UserLog> UserLogs { get; set; } = null!;
    
	// Configuration
	private readonly LogOnlyInsertInterceptor _logOnlyInsertInterceptor;
	
	public LogDbContext(DbContextOptions<LogDbContext> options, LogOnlyInsertInterceptor logOnlyInsertInterceptor) : base(options)
	{
		_logOnlyInsertInterceptor = logOnlyInsertInterceptor;
		
		ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		ChangeTracker.AutoDetectChangesEnabled = false;
	}
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		var userLogEntity = modelBuilder.Entity<UserLog>();
		userLogEntity.HasKey(x => x.Id);
		userLogEntity.Property(x => x.Id).ValueGeneratedOnAdd();
		userLogEntity.Property(x => x.Action).HasMaxLength(50).IsRequired();
		userLogEntity.Property(x => x.Details).HasMaxLength(512).IsRequired();
		userLogEntity.Property(x => x.IpAddress).HasMaxLength(20).IsRequired();
		userLogEntity.Property(x => x.UserAgent).HasMaxLength(512).IsRequired();
		userLogEntity.Property(x => x.UserEmail).HasMaxLength(50);
		userLogEntity.HasIndex(x => x.Id);
		userLogEntity.HasIndex(x => x.Timestamp)
			.IncludeProperties(x => new { x.Id, x.Action, x.IsSuccess, x.Details, x.IpAddress, x.UserAgent, x.UserId, x.UserEmail });
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		optionsBuilder.AddInterceptors(_logOnlyInsertInterceptor);
	}
}