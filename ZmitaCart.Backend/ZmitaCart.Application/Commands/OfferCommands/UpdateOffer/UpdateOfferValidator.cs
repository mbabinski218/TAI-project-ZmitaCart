using FluentValidation;
using ZmitaCart.Domain.Common.Types;

namespace ZmitaCart.Application.Commands.OfferCommands.UpdateOffer;

public class UpdateOfferValidator : AbstractValidator<UpdateOfferCommand>
{
	public UpdateOfferValidator()
	{
		RuleFor(o => o.Id)
			.NotNull().WithMessage("Enter offer id")
			.NotEmpty().WithMessage("Enter offer id");
		
		RuleFor(o => o.Title)
			.MaximumLength(50).WithMessage("Title must not exceed 50 characters");
		
		RuleFor(o => o.Description)
			.MaximumLength(500).WithMessage("Description must not exceed 500 characters");

		RuleFor(o => o.Price)
			.GreaterThan(0).WithMessage("Price must be greater than 0");
		
		RuleFor(o=>o.Quantity)
			.GreaterThan(0).WithMessage("Quantity must be greater than 0");

		RuleFor(o => o.Condition)
			.Must(c => Condition.SupportedConditions.Contains(c))
			.When(o => o.Condition is not null).WithMessage("Condition must be one of the following: New, Used, Good");
	}
}