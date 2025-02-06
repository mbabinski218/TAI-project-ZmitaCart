using FluentValidation;

namespace ZmitaCart.Application.Commands.UserCommands.GiveFeedback;

public class GiveFeedbackValidator : AbstractValidator<GiveFeedbackCommand>
{
	public GiveFeedbackValidator()
	{
		RuleFor(f => f.Rating)
			.NotNull().WithMessage("Rating is required")
			.InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");

		RuleFor(f => f.Comment)
			.MaximumLength(500).WithMessage("Comment must be less than 500 characters");
	}
}