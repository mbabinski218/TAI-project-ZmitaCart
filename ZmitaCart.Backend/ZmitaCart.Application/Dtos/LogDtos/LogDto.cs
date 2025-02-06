namespace ZmitaCart.Application.Dtos.LogDtos;

public sealed class LogDto
{
	public int Id { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public string Action { get; set; } = null!;
	public bool IsSuccess { get; set; }
	public string Details { get; set; } = null!;
	public string IpAddress { get; set; } = null!;
	public string UserAgent { get; set; } = null!;
	public int? UserId { get; set; }
	public string? UserEmail { get; set; }
}