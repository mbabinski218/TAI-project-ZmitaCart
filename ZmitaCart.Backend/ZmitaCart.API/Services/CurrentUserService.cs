using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.Extensions;
using ZmitaCart.API.Common;
using ZmitaCart.Application.Interfaces.Services;
using ZmitaCart.Domain.Common.Types;

namespace ZmitaCart.API.Services;

public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public CurrentUserService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public JwtSecurityToken? UserToken => Reader.ReadToken(_httpContextAccessor.HttpContext);
	public string? UserId => UserToken?.FindOrDefault(ClaimNames.Id);
	public string? UserRole => UserToken?.FindOrDefault(ClaimNames.Role);
	public Uri? Uri
	{
		get
		{
			try
			{
				var url = _httpContextAccessor.HttpContext?.Request.GetDisplayUrl();
				return url is null ? null : new Uri(url);
			}
			catch
			{
				return null;
			}
		}
	}
	public string? IpAddress => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
	public string? UserAgent => _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString();
}