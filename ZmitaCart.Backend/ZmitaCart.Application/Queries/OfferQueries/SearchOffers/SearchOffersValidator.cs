using FluentValidation;
using ZmitaCart.Domain.Common.Types;

namespace ZmitaCart.Application.Queries.OfferQueries.SearchOffers;

public class SearchOffersValidator : AbstractValidator<SearchOffersQuery>
{
	public SearchOffersValidator()
	{
		RuleFor(s=>s.Conditions)
			.Must(x => x!.Any(c => Condition.SupportedConditions.Contains(c)))
			.When(s => s.Conditions is not null)
			.WithMessage($"Condition must be one of the following: {string.Join(", ", Condition.SupportedConditions)}");

		RuleFor(s => s.MaxPrice)
			.GreaterThanOrEqualTo(0).WithMessage("MaxPrice must be greater than or equal to 0");
		
		RuleFor(s => s.MinPrice)
			.GreaterThanOrEqualTo(0).WithMessage("MinPrice must be greater than or equal to 0");

		RuleFor(s => s.SortBy)
		 	.Must(x => Sort.SupportedSorts.Contains(x))
		    .When(s => s.SortBy is not null)
		    .WithMessage($"Sort must be one of the following: {string.Join(", ", Sort.SupportedSorts)}");

		RuleFor(s => s.PageNumber)
			.GreaterThanOrEqualTo(1).WithMessage("PageNumber must be greater than or equal to 1")
			.Must(x => x.HasValue).When(s => s.PageSize.HasValue).WithMessage("PageNumber is required");


		RuleFor(s => s.PageSize)
			.GreaterThanOrEqualTo(1).WithMessage("PageSize must be greater than or equal to 1")
			.Must(x => x.HasValue).When(s => s.PageNumber.HasValue).WithMessage("PageSize is required");
		
	}
}