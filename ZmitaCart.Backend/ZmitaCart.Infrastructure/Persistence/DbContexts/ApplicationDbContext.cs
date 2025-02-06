using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Domain.Common.Models;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Persistence.Interceptors;

namespace ZmitaCart.Infrastructure.Persistence.DbContexts;

public class ApplicationDbContext : IdentityDbContext<User, IdentityUserRole, int>
{ 
    // DbSet
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Offer> Offers { get; set; } = null!;
    public DbSet<UserOffer> Favorites { get; set; } = null!;
    public DbSet<Bought> Bought { get; set; } = null!;
    public DbSet<Feedback> Feedbacks { get; set; } = null!;
    public DbSet<Picture> Pictures { get; set; } = null!;
    
    // Configuration
    private readonly DateTimeSetterInterceptor _dateTimeSetterInterceptor;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, DateTimeSetterInterceptor dateTimeSetterInterceptor) : base(options)
    {
        _dateTimeSetterInterceptor = dateTimeSetterInterceptor;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<IdentityRoleClaim<int>>();
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(_dateTimeSetterInterceptor);
    }
}
