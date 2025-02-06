using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.CategoryDtos;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Queries.CategoryQueries.GetParentCategory;

public class GetParentCategoryHandler : IRequestHandler<GetParentCategoryQuery, Result<CategoryDto?>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetParentCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryDto?>> Handle(GetParentCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetParentCategory(request.ParentId);
    }
}