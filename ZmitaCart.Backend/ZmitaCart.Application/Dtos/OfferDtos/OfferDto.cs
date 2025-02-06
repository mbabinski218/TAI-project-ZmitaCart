using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Domain.ValueObjects;

namespace ZmitaCart.Application.Dtos.OfferDtos;

public class OfferDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public bool IsAvailable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Condition { get; set; } = null!;
    public int CategoryId { get; set; }
    public UserDto User { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public ICollection<string>? PicturesNames { get; set; }
    public bool IsFavourite { get; set; }
}