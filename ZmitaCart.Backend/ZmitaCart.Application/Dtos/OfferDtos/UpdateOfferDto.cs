namespace ZmitaCart.Application.Dtos.OfferDtos;

public class UpdateOfferDto
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public string? Title { get; set; }
	public string? Description { get; set; }
	public decimal? Price { get; set; }
	public int? Quantity { get; set; }
	public bool? IsAvailable { get; set; }
	public string? Condition { get; set; }
}