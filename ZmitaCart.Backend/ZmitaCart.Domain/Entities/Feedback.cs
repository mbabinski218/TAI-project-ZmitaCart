using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public class Feedback : Entity<int>
{
	public int RecipientId { get; set; }
	public int RaterId { get; set; }
	public User Rater { get; set; } = null!;
	public int Rating { get; set; }
	public string? Comment { get; set; }
}