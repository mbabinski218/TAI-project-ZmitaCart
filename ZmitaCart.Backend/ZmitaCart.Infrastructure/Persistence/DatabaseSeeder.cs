using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Domain.Common.Models;
using ZmitaCart.Domain.Common.Types;
using ZmitaCart.Infrastructure.Persistence.DbContexts;

namespace ZmitaCart.Infrastructure.Persistence;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RoleManager<IdentityUserRole> _roleManager;

    public DatabaseSeeder(ApplicationDbContext dbContext, RoleManager<IdentityUserRole> roleManager)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
    }

    public async Task Seed()
    {
        if (!await _dbContext.Database.CanConnectAsync())
        {
            return;
        }

        await SeedRoles();
        
        if (!await _dbContext.Users.AnyAsync())
        {
            await SeedData("Users.txt");
        }
        
        if (!await _dbContext.Categories.AnyAsync())
        {
            await SeedData("Categories.txt");
        }
        
        if (!await _dbContext.Offers.AnyAsync())
        {
            await SeedData("Offers.txt");
        }

        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedRoles()
    {
        foreach (var supportedRole in Role.SupportedRoles)
        {
            var role = new IdentityUserRole(supportedRole);

            if (!_dbContext.Roles.Contains(role))
            {
                await _roleManager.CreateAsync(role);
            }
        }
    }

    private async Task SeedData(string fileName)
    {
        var basePath = Directory.GetCurrentDirectory();
        var pathToFile = Path.Combine(basePath, $@"..\\ZmitaCart.Infrastructure\Persistence\{fileName}");
        var script = await File.ReadAllTextAsync(pathToFile);

        await _dbContext.Database.ExecuteSqlRawAsync(script);
    }
}