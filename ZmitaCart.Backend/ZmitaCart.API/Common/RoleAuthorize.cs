using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ZmitaCart.Domain.Common.Types;

namespace ZmitaCart.API.Common;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RoleAuthorize : AuthorizeAttribute, IAuthorizationFilter
{
	private readonly IEnumerable<string> _roles;
	public RoleAuthorize(params string[] roles)
	{
		_roles = roles.Length == 0 ? Role.SupportedRoles : roles;
	}

	public void OnAuthorization(AuthorizationFilterContext context)
	{
		var token = Reader.ReadToken(context.HttpContext);
		var userRole = token?.FindOrDefault(ClaimNames.Role);

		if (userRole is null || !_roles.Contains(userRole))
		{
			context.Result = new ObjectResult(new List<string> { "You are not authorized to access this resource" })
			{
				StatusCode = StatusCodes.Status401Unauthorized
			};
		}
	}
}