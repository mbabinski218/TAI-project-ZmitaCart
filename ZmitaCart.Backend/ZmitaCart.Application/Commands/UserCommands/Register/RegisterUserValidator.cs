using FluentValidation;

namespace ZmitaCart.Application.Commands.UserCommands.Register;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
	public RegisterUserValidator()
	{
		RuleFor(u => u.Email)
			.NotEmpty().WithMessage("Enter email address")
			.EmailAddress().WithMessage("Wrong email format");

		RuleFor(u => u.FirstName)
			.NotEmpty().WithMessage("Enter first name");		
		
		RuleFor(u => u.LastName)
			.NotEmpty().WithMessage("Enter last name");

		RuleFor(u => u.Password)
			.NotEmpty().WithMessage("Enter password")
			.Equal(u => u.ConfirmedPassword).WithMessage("Passwords must be the same");

		RuleFor(u => u.ConfirmedPassword)
			.NotEmpty().WithMessage("Confirm password");
	}
}