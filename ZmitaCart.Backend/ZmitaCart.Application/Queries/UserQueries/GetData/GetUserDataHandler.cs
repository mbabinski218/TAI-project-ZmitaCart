using FluentResults;
using MediatR;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Queries.UserQueries.GetData;

public class GetUserDataHandler : IRequestHandler<GetDataQuery, Result<UserDataDto>>
{
	private readonly IUserRepository _userRepository;
	private readonly ICurrentUserService _currentUserService;

	public GetUserDataHandler(IUserRepository userRepository, ICurrentUserService currentUserService)
	{
		_userRepository = userRepository;
		_currentUserService = currentUserService;
	}

	public async Task<Result<UserDataDto>> Handle(GetDataQuery request, CancellationToken cancellationToken)
	{
		if (_currentUserService.UserId is null)
		{
			return Result.Fail(new UnauthorizedError("You are not logged in"));
		}
		
		var userId = int.Parse(_currentUserService.UserId);

		return await _userRepository.GetDataAsync(userId);
	}
}