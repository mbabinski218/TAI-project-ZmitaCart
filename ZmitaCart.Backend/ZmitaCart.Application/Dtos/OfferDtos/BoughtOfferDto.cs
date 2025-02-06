namespace ZmitaCart.Application.Dtos.OfferDtos;

public class BoughtOfferDto
{
	public OfferInfoDto Offer { get; set; } = null!;
	public DateTimeOffset BoughtAt { get; set; }
	public int BoughtQuantity { get; set; }
	public decimal TotalPrice { get; set; }
}