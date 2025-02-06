namespace ZmitaCart.Domain.Common.Types;

public static class Sort
{
	public const string priceAscending = "PriceAscending";
	public const string priceDescending = "PriceDescending";
	public const string createdAscending = "CreatedAscending";
	public const string createdDescending = "CreatedDescending";
	
	public static IEnumerable<string> SupportedSorts
	{
		get
		{
			yield return priceAscending;
			yield return priceDescending;
			yield return createdAscending;
			yield return createdDescending;
		}
	}
}