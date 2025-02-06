using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZmitaCart.Infrastructure.Common.Settings;

namespace ZmitaCart.Infrastructure.Common;

public class JwtHelper
{
	private readonly JwtSettings _jwtSettings;

	public JwtHelper(IOptions<JwtSettings> jwtSettings)
	{
		_jwtSettings = jwtSettings.Value;
	}

	public string GenerateAccessToken(IEnumerable<Claim> userClaims)
	{
		var signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), 
			SecurityAlgorithms.HmacSha512Signature);

		var token = new JwtSecurityToken
		(
			claims: userClaims,
			issuer: _jwtSettings.Issuer,
			audience: _jwtSettings.Audience,
			expires: DateTime.Now.AddMinutes(_jwtSettings.ExpireTimeInMinutes),
			signingCredentials: signingCredentials
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public string GenerateRefreshToken()
	{
		return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
	}
}