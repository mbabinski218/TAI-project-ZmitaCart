namespace ZmitaCart.Application.Dtos.OfferDtos;

public class CreateOfferDto
{
	public string Title { get; set; } = null!;
	public string Description { get; set; } = null!;
	public decimal Price { get; set; }
	public int Quantity { get; set; }
	public bool IsAvailable { get; set; }
	public DateTimeOffset CreatedAt { get; set; }
	public string Condition { get; set; } = null!;
	public int CategoryId { get; set; }
	public int UserId { get; set; }
}