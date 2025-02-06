using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.ValueObjects;

public class Address : ValueObject
{
	public string? Country { get; set; }
	public string? City { get; set; }
	public string? Street { get; set; }
	public int? PostalCode { get; set; }
	public int? HouseNumber { get; set; }
	public int? ApartmentNumber { get; set; }

	protected override IEnumerable<object?> GetEqualityComponents()
	{
		yield return Country;
		yield return City;
		yield return Street;
		yield return PostalCode;
		yield return HouseNumber;
		yield return ApartmentNumber;
	}
}