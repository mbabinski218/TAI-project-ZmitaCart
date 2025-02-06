using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.LogDtos;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Queries.LogQueries.GetLogs;

public class GetLogsHandler : IRequestHandler<GetLogsQuery, Result<PaginatedList<LogDto>>>
{
	private readonly IUserRepository _userRepository;
	
	public GetLogsHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<Result<PaginatedList<LogDto>>> Handle(GetLogsQuery request, CancellationToken cancellationToken)
	{
		return await _userRepository.GetLogsAsync(request.SearchText, request.IsSuccess, request.From,
			request.To, request.PageNumber, request.PageSize);
	}
}