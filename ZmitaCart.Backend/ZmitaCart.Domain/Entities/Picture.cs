using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class Picture : Entity<int>
{
    public int OfferId { get; set; }
    public virtual Offer Offer { get; set; } = null!;
    public string Name { get; set; } = null!;
}