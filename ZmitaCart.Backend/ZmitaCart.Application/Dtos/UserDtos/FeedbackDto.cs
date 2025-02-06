namespace ZmitaCart.Application.Dtos.UserDtos;

public class FeedbackDto
{
	public int Id { get; set; }
	public string RaterName { get; set; } = null!;
	public int Rating { get; set; }
	public string? Comment { get; set; }
}