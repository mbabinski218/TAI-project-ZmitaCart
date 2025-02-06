using FluentValidation;

namespace ZmitaCart.Application.Commands.UserCommands.ResendEmailConfirmation;

public sealed class ResendEmailConfirmationValidator : AbstractValidator<ResendEmailConfirmationCommand>
{
	public ResendEmailConfirmationValidator()
	{
		RuleFor(u => u.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Email is not valid.");
	}
}