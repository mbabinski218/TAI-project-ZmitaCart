using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class Bought : Entity<int>
{
	public int OfferId { get; set; }
	public virtual Offer Offer { get; set; } = null!;
	public int UserId { get; set; }
	public virtual User User { get; set; } = null!;
	public int Quantity { get; set; }
	public DateTimeOffset BoughtAt { get; set; }
	public decimal TotalPrice { get; set; }
}