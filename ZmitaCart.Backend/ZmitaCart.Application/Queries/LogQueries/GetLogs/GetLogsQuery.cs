using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.LogDtos;

namespace ZmitaCart.Application.Queries.LogQueries.GetLogs;

public record GetLogsQuery (
		string? SearchText,
		bool? IsSuccess,
		DateTimeOffset? From,
		DateTimeOffset? To,
		int? PageNumber, 
		int? PageSize
	) : IRequest<Result<PaginatedList<LogDto>>>;