using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Domain.Common.Models;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Common;
using ZmitaCart.Infrastructure.Common.Settings;
using ZmitaCart.Infrastructure.Persistence;
using ZmitaCart.Infrastructure.Persistence.DbContexts;
using ZmitaCart.Infrastructure.Persistence.Interceptors;
using ZmitaCart.Infrastructure.Repositories;
using ZmitaCart.Infrastructure.Services;

namespace ZmitaCart.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string applicationDbName)
	{
		AddPersistence(services, configuration, applicationDbName);
		AddAuthentication(services, configuration);
		AddRepositories(services);
		services.AddAuthorization();

		return services;
	}

	private static IServiceCollection AddRepositories(IServiceCollection services)
	{
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<ICategoryRepository, CategoryRepository>();
		services.AddScoped<IOfferRepository, OfferRepository>();
		services.AddScoped<IPictureRepository, PictureRepository>();
		services.AddScoped<GoogleAuthentication>();

		return services;
	}

	private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, string applicationDbName)
	{
		services.AddDbContext<LogDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString(applicationDbName)!, 
				sqlOpt => sqlOpt.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null)));
		
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(configuration.GetConnectionString(applicationDbName)!).EnableDetailedErrors());

		services.AddIdentityCore<User>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.SignIn.RequireConfirmedEmail = true;
				
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 8;
				options.Password.RequiredUniqueChars = 0;
			})
			.AddRoles<IdentityUserRole>()
			.AddDefaultTokenProviders()
			.AddEntityFrameworkStores<ApplicationDbContext>();
		
		services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
		services.AddScoped<IUserEventLoggerService, UserEventLoggerService>();
		services.AddScoped<DateTimeSetterInterceptor>();
		services.AddScoped<LogOnlyInsertInterceptor>();

		return services;
	}

	private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		var jwtSettings = new JwtSettings();
		configuration.Bind(JwtSettings.sectionName, jwtSettings);
		services.AddSingleton(Options.Create(jwtSettings));

		var googleSettings = new GoogleSettings();
		configuration.Bind(GoogleSettings.sectionName, googleSettings);
		services.AddSingleton(Options.Create(googleSettings));

		services.AddSingleton<JwtHelper>();

		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					RequireAudience = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					ValidIssuer = jwtSettings.Issuer,
					ValidAudience = jwtSettings.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
				};
			})
			.AddGoogle(options =>
			{
				options.ClientId = googleSettings.ClientId;
				options.ClientSecret = googleSettings.ClientSecret;
			});

		return services;
	}
}