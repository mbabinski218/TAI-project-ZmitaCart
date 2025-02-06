namespace ZmitaCart.Application.Dtos.OfferDtos;

public class OfferDataDto
{
	public string Title { get; set; } = null!;
	public string Description { get; set; } = null!;
	public decimal Price { get; set; }
	public int Quantity { get; set; }
	public string Condition { get; set; } = null!;
	public int CategoryId { get; set; }
	public IEnumerable<string>? PicturesNames { get; set; }
}