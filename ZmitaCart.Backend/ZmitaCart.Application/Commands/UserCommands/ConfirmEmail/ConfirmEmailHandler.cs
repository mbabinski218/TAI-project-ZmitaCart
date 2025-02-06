using System.Web;
using FluentResults;
using MediatR;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Commands.UserCommands.ConfirmEmail;

public sealed class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Result>
{
	private readonly IUserRepository _userRepository;

	public ConfirmEmailHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.FindByIdAsync(request.Id);

		if (user.IsFailed)
		{
			return user.ToResult();
		}
		
		var token = request.Token;
		token = token.Replace("-", "+").Replace("_", "/");
		var padding = 4 - token.Length % 4;
		if (padding < 4) token = token.PadRight(token.Length + padding, '=');

		var bytes = Convert.FromBase64String(token);
		var decodedToken = System.Text.Encoding.UTF8.GetString(bytes);
		
		return await _userRepository.ConfirmEmailAsync(user.Value, decodedToken);
	}
}