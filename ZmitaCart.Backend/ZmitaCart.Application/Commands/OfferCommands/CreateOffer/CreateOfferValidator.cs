using FluentValidation;
using ZmitaCart.Domain.Common.Types;

namespace ZmitaCart.Application.Commands.OfferCommands.CreateOffer;

public class CreateOfferValidator : AbstractValidator<CreateOfferCommand>
{
	public CreateOfferValidator()
	{
		RuleFor(o => o.Title)
			.NotEmpty().WithMessage("Enter a title")
			.MaximumLength(50).WithMessage("Title must not exceed 50 characters");
		
		RuleFor(o => o.Description)
			.NotEmpty().WithMessage("Enter a description")
			.MaximumLength(500).WithMessage("Description must not exceed 500 characters");

		RuleFor(o => o.Price)
			.NotEmpty().WithMessage("Enter a price")
			.GreaterThan(0).WithMessage("Price must be greater than 0");
		
		RuleFor(o=>o.Quantity)
			.NotEmpty().WithMessage("Enter a quantity")
			.GreaterThan(0).WithMessage("Quantity must be greater than 0");
		
		RuleFor(o=>o.Condition)
			.NotEmpty().WithMessage("Enter a condition")
			.Must(c => Condition.SupportedConditions.Contains(c)).WithMessage("Condition must be one of the following: New, Used, Good");

		RuleFor(o => o.CategoryId)
			.NotEmpty().WithMessage("Enter a category");
	}
}