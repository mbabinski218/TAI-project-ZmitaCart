namespace ZmitaCart.Domain.Common.Types;

public static class Role
{
	public const string administrator = "Administrator";
	public const string user = "User";

	public static IEnumerable<string> SupportedRoles
	{
		get
		{
			yield return administrator;
			yield return user;
		}
	}
}