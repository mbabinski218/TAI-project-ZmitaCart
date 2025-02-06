using FluentResults;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.LogDtos;

namespace ZmitaCart.Infrastructure.Services;

public interface IUserEventLoggerService
{
	Task LogUserLoggedInSuccessAsync(string details, int userId, string userEmail);
	Task LogUserLoggedInFailureAsync(string details, string? userEmail);
	Task LogUserLoggedOutSuccessAsync(string details, int userId, string userEmail);
	Task LogUserLoggedOutFailureAsync(string details, int userId);
	Task LogUserRegisteredSuccessAsync(string details, int userId, string userEmail);
	Task LogUserRegisteredFailureAsync(string details, string userEmail);
	Task<Result<PaginatedList<LogDto>>> GetLogsAsync(string? searchText, bool? isSuccess,
		DateTimeOffset? from, DateTimeOffset? to, int? pageNumber, int? pageSize);
}