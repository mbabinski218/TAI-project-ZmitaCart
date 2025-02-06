using FluentResults;
using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;
using ZmitaCart.Domain.Common;

namespace ZmitaCart.Application.Commands.UserCommands.LoginUser;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<TokensDto>>
{
	private readonly IUserRepository _userRepository;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public LoginUserHandler(IUserRepository userRepository, IMapper mapper, ICurrentUserService currentUserService)
	{
		_userRepository = userRepository;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}
	
	public async Task<Result<TokensDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
	{
		return request.GrantType switch
		{
			GrantType.password => await _userRepository.LoginAsync(_mapper.Map<LoginUserDto>(request)),
			GrantType.refreshToken => await _userRepository.LoginWithRefreshTokenAsync(request.RefreshToken!),
			GrantType.google => await _userRepository.LoginWithGoogleAsync(request.IdToken!),
			_ => Result.Fail(new InvalidDataError("Invalid provider"))
		};
	}
}