using FluentResults;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Options;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Commands.UserCommands.Register;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;
	private readonly IEmailServiceProvider _emailServiceProvider;

	public RegisterUserHandler(IUserRepository userRepository, IMapper mapper, 
		ICurrentUserService currentUserService, IEmailServiceProvider emailServiceProvider)
	{
		_userRepository = userRepository;
		_mapper = mapper;
		_currentUserService = currentUserService;
		_emailServiceProvider = emailServiceProvider;
	}

	public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var registerUser = _mapper.Map<RegisterUserDto>(request);

		var user = await _userRepository.RegisterAsync(registerUser);
		if (user.IsFailed)
		{
			return user.ToResult();
		}
		
		var token = await _userRepository.GenerateEmailConfirmationTokenAsync(user.Value);

		if (token.IsFailed)
		{
			return Result.Fail("Token generation failed");
		}
		
		var emailConfirmationLink = EmailHelper.CreateConfirmationLink(user.Value.Id, token.Value);
		
		await _emailServiceProvider.SendEmailConfirmationAsync(request.Email, emailConfirmationLink, cancellationToken);
		
		return Result.Ok();
	}
}