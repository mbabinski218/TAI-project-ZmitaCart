namespace ZmitaCart.Domain.Common.Models;

public interface IEntity
{
	public DateTimeOffset CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }
}