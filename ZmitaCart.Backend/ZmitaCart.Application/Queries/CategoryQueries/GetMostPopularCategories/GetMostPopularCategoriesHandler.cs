using FluentResults;
using MediatR;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Queries.CategoryQueries.GetMostPopularCategories;

public class GetMostPopularCategoriesHandler : IRequestHandler<GetMostPopularCategoriesQuery, Result<List<string>>>
{
	private readonly ICategoryRepository _categoryRepository;

	public GetMostPopularCategoriesHandler(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task<Result<List<string>>> Handle(GetMostPopularCategoriesQuery request, CancellationToken cancellationToken)
	{
		return await _categoryRepository.GetMostPopularCategoriesAsync(request.NumberOfCategories);
	}
}