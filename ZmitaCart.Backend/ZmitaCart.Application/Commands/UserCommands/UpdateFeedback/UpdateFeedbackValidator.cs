using FluentValidation;

namespace ZmitaCart.Application.Commands.UserCommands.UpdateFeedback;

public class UpdateFeedbackValidator : AbstractValidator<UpdateFeedbackCommand>
{
	public UpdateFeedbackValidator()
	{
		RuleFor(f => f.Rating)
			.InclusiveBetween(1, 5).When(f => f.Rating is not null).WithMessage("Rating must be between 1 and 5");

		RuleFor(f => f.Comment)
			.MaximumLength(500).WithMessage("Comment must be less than 500 characters");
	}
}