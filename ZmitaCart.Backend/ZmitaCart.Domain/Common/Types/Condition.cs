namespace ZmitaCart.Domain.Common.Types;

public static class Condition
{
	public const string brandNew = "Nowy";
	public const string used = "Używany";
	public const string good = "Dobry";
	
	public static IEnumerable<string> SupportedConditions
	{
		get
		{
			yield return brandNew;
			yield return used;
			yield return good;
		}
	}
}