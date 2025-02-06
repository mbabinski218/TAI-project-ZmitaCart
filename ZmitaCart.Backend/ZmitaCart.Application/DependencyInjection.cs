using System.Reflection;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ZmitaCart.Application.Behaviors;

namespace ZmitaCart.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
#if _debug
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
#endif
		});
		
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		services.AddMapper();

		return services;
	}

	private static IServiceCollection AddMapper(this IServiceCollection services)
	{
		var config = TypeAdapterConfig.GlobalSettings;
		config.Scan(Assembly.GetExecutingAssembly());
		
		services.AddSingleton(config);
		services.AddScoped<IMapper, ServiceMapper>();
		
		return services;
	}
}