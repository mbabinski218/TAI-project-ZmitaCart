using FluentResults;
using MediatR;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Application.Interfaces.Services;

namespace ZmitaCart.Application.Commands.UserCommands.ResendEmailConfirmation;

public sealed class ResendEmailConfirmationHandler : IRequestHandler<ResendEmailConfirmationCommand, Result>
{
	private readonly IUserRepository _userRepository;
	private readonly ICurrentUserService _currentUserService;
	private readonly IEmailServiceProvider _emailServiceProvider;

	public ResendEmailConfirmationHandler(IUserRepository userRepository, ICurrentUserService currentUserService,
		IEmailServiceProvider emailServiceProvider)
	{
		_userRepository = userRepository;
		_currentUserService = currentUserService;
		_emailServiceProvider = emailServiceProvider;
	}

	public async Task<Result> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.FindByEmailAsync(request.Email);
		
		if (user.IsFailed)
		{
			return Result.Fail("User not found");
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