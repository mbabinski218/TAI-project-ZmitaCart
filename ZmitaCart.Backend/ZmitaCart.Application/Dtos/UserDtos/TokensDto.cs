namespace ZmitaCart.Application.Dtos.UserDtos;

public record TokensDto
{
	public string AccessToken { get; init; } = null!;
	public string RefreshToken { get; init; } = null!;
}