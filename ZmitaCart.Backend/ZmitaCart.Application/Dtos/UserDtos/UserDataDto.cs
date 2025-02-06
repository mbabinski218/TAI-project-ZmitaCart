using ZmitaCart.Domain.ValueObjects;

namespace ZmitaCart.Application.Dtos.UserDtos;

public class UserDataDto
{
	public string Email { get; set; } = null!;
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string? PhoneNumber { get; set; }
	public Address? Address { get; set; }
}