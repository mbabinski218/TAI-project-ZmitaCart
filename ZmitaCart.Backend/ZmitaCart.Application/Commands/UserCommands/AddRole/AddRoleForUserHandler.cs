using FluentResults;
using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Commands.UserCommands.AddRole;

public class AddRoleForUserHandler : IRequestHandler<AddRoleForUserCommand, Result>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;

	public AddRoleForUserHandler(IUserRepository userRepository, IMapper mapper, ICurrentUserService currentUserService)
	{
		_userRepository = userRepository;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}
	
	public async Task<Result> Handle(AddRoleForUserCommand request, CancellationToken cancellationToken)
	{
		return await _userRepository.AddRoleAsync(request.UserEmail!, request.Role!);
	}
}