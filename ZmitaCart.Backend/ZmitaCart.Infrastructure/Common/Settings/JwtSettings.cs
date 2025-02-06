namespace ZmitaCart.Infrastructure.Common.Settings;

public class JwtSettings
{
	public const string sectionName = "JwtSettings";
	public string Secret { get; init; } = null!;
	public int ExpireTimeInMinutes { get; init; }
	public string Issuer { get; init; } = null!;
	public string Audience { get; init; } = null!;
}