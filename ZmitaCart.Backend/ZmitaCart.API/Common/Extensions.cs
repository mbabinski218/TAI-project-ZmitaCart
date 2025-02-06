using System.IdentityModel.Tokens.Jwt;

namespace ZmitaCart.API.Common;

public static class JwtSecurityTokenExtensions
{
	public static string? FindOrDefault(this JwtSecurityToken? token, string claimName) => 
		token?.Claims.FirstOrDefault(c => c.Type == claimName)?.Value;
}

public static class Reader
{
	public static JwtSecurityToken? ReadToken(HttpContext? httpContext)
	{
		var token = httpContext?.Request.Headers["Authorization"]
			.FirstOrDefault()
			?.Split(" ")
			.LastOrDefault();

		if (token is null)
			return null;
		
		var handler = new JwtSecurityTokenHandler();
		
		return handler.CanReadToken(token) ? handler.ReadJwtToken(token) : null;
	}
}

public static class HealthCheckExtensions
{
	public static IHealthChecksBuilder AddSqlServerCheck(this IHealthChecksBuilder builder, string? connectionString, string name)
	{
		builder.AddSqlServer(connectionString!, name: name);
		return builder;
	}
}
