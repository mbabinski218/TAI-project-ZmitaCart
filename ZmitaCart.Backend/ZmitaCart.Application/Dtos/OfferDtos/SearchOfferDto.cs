namespace ZmitaCart.Application.Dtos.OfferDtos;

public class SearchOfferDto
{
	public string? Title { get; set; }
	public int? CategoryId { get; set; }
	public IEnumerable<string>? Conditions { get; set; }
	public decimal? MinPrice { get; set; }
	public decimal? MaxPrice { get; set; }
	public string? SortBy { get; set; }
	public int? UserId { get; set; }
}