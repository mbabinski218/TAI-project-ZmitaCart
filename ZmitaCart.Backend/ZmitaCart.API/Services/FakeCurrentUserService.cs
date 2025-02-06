using System.IdentityModel.Tokens.Jwt;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.API.Services;

public class FakeCurrentUserService : ICurrentUserService
{
	public JwtSecurityToken? UserToken => new();
	public string? UserId => "1";
	public string? UserRole => "Admin";
	public Uri? Uri => new("http://localhost:4200/");
	public string? IpAddress => "127.0.0.1";
	public string? UserAgent => "Example User Agent";
}