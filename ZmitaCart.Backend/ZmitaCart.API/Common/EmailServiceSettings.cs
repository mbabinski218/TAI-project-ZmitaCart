namespace ZmitaCart.API.Common;

public sealed class EmailServiceSettings
{
	public const string sectionName = "EmailService";
	public string Host { get; init; } = null!;
	public int Port { get; init; }
	public string Email { get; init; } = null!;
	public string Password { get; init; } = null!;
}