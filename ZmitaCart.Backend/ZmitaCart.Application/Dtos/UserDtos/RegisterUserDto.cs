namespace ZmitaCart.Application.Dtos.UserDtos;

public record RegisterUserDto
{
	public string Email { get; set; } = null!;
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string Password { get; set; } = null!;
	public bool? IsAdmin { get; set; }
}