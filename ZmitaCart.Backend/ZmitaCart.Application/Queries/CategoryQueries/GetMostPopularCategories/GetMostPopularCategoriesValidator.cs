using FluentValidation;

namespace ZmitaCart.Application.Queries.CategoryQueries.GetMostPopularCategories;

public class GetMostPopularCategoriesValidator : AbstractValidator<GetMostPopularCategoriesQuery>
{
	public GetMostPopularCategoriesValidator()
	{
		RuleFor(x => x.NumberOfCategories)
			.GreaterThanOrEqualTo(1).WithMessage("NumberOfCategories must be greater than or equal to 1");
	}
}