using FluentValidation;
using ZmitaCart.Domain.Common;

namespace ZmitaCart.Application.Commands.UserCommands.LoginUser;

public class LoginUserValidator : AbstractValidator<LoginUserCommand>
{
	public LoginUserValidator()
	{
		RuleFor(l=>l.GrantType)
			.NotEmpty().WithMessage("Enter authenticator")
			.Must(x => GrantType.SupportedGrantTypes.Contains(x)).WithMessage("Invalid provider");

		RuleFor(l => l.Email)
			.Must(x => x is not null)
			.When(x => x.GrantType is GrantType.password || x.Password is not null)
			.Must(x => x is null)
			.When(x => x.RefreshToken is not null || x.IdToken is not null)
			.EmailAddress().WithMessage("Wrong email format");

		RuleFor(x => x.Password)
			.Must(x => x is not null)
			.When(x => x.GrantType is GrantType.password || x.Email is not null)
			.Must(x => x is null)
			.When(x => x.RefreshToken is not null || x.IdToken is not null);
		
		RuleFor(x => x.RefreshToken)
			.Must(x => x is not null)
			.When(x => x.GrantType is GrantType.refreshToken)
			.Must(x => x is null)
			.When(x => x.IdToken is not null || x.Password is not null || x.Email is not null);
		
		RuleFor(x => x.IdToken)
			.Must(x => x is null)
			.When(x => x.RefreshToken is not null || x.Password is not null || x.Email is not null);
	}
}