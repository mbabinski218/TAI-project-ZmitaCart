using System.IdentityModel.Tokens.Jwt;

namespace ZmitaCart.Application.Interfaces.Services;

public interface ICurrentUserService
{
	public JwtSecurityToken? UserToken { get; }
	public string? UserId { get; }
	public string? UserRole { get; }
	public Uri? Uri { get; }
	public string? IpAddress { get; }
	public string? UserAgent { get; }
}