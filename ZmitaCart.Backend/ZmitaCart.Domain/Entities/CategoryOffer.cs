namespace ZmitaCart.Domain.Entities;

public class CategoryOffer
{
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public int OfferId { get; set; }
    public Offer Offer { get; set; } = null!;
}