namespace ZmitaCart.Application.Dtos.OfferDtos;

public class OfferInfoDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public string City { get; set; } = null!;
    public string Condition { get; set; } = null!;
    public int Quantity { get; set; }
    public string? ImageName { get; set; }
    public bool IsFavourite { get; set; }
    public bool IsAvailable { get; set; }
    public string AuthorName { get; set; } = null!;
    public string AuthorEmail { get; set; } = null!;
}
