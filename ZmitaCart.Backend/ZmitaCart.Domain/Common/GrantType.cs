namespace ZmitaCart.Domain.Common;

public static class GrantType
{
	public const string password = "password";
	public const string refreshToken = "refresh_token";
	public const string google = "google";
	
	public static IEnumerable<string> SupportedGrantTypes
	{
		get
		{
			yield return password;
			yield return refreshToken;
			yield return google;
		}
	}
}