using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Domain.Entities;

public sealed class UserLog : ILogEntity
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
	
	private static UserLog Create(string action, bool isSuccess, string details, string ipAddress, string userAgent, int? userId, string? userEmail)
		=> new()
		{
			Timestamp = DateTimeOffset.UtcNow,
			Action = action,
			IsSuccess = isSuccess,
			Details = details,
			IpAddress = ipAddress,
			UserAgent = userAgent,
			UserId = userId,
			UserEmail = userEmail
		};
	
	public static UserLog CreateLoginSuccess(string details, string ipAddress, string userAgent, int? userId, string? userEmail)
		=> Create("Login", true, details, ipAddress, userAgent, userId, userEmail);
	
	public static UserLog CreateLoginFailed(string details, string ipAddress, string userAgent, string? userEmail)
		=> Create("Login", false, details, ipAddress, userAgent, null, userEmail);
	
	public static UserLog CreateLogoutSuccess(string details, string ipAddress, string userAgent, int? userId, string? userEmail)
		=> Create("Logout", true, details, ipAddress, userAgent, userId, userEmail);
	
	public static UserLog CreateLogoutFailed(string details, string ipAddress, string userAgent, int? userId)
		=> Create("Logout", true, details, ipAddress, userAgent, userId, null);
	
	public static UserLog CreateRegisterSuccess(string details, string ipAddress, string userAgent, int? userId, string? userEmail)
		=> Create("Register", true, details, ipAddress, userAgent, userId, userEmail);
	
	public static UserLog CreateRegisterFailed(string details, string ipAddress, string userAgent, string? userEmail)
		=> Create("Register", false, details, ipAddress, userAgent, null, userEmail);
}