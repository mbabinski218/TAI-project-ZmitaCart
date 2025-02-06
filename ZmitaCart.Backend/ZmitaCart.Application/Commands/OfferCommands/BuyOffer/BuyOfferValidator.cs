using FluentValidation;

namespace ZmitaCart.Application.Commands.OfferCommands.BuyOffer;

public class BuyOfferValidator : AbstractValidator<BuyOfferCommand>
{
	public BuyOfferValidator()
	{
		RuleFor(x => x.OfferId)
			.NotNull().WithMessage("OfferId is required.")
			.NotEmpty().WithMessage("OfferId is required.");
		
		RuleFor(x => x.Quantity)
			.GreaterThan(0).WithMessage("Quantity must be greater than 0.");
	}
}