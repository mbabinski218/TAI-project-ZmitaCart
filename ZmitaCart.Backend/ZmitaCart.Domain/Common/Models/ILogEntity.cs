namespace ZmitaCart.Domain.Common.Models;

public interface ILogEntity
{
	int Id { get; set; }
	DateTimeOffset Timestamp { get; set; }
	string Action { get; set; }
}