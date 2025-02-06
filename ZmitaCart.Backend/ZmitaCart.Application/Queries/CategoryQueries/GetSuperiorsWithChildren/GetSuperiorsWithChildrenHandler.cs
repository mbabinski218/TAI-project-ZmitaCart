using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.CategoryDtos;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Queries.CategoryQueries.GetSuperiorsWithChildren;

public class GetSuperiorsWithChildrenHandler : IRequestHandler<GetSuperiorsWithChildrenQuery, Result<IEnumerable<CategoryDto>>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetSuperiorsWithChildrenHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetSuperiorsWithChildrenQuery request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetSuperiorsWithFewChildren(request.ChildrenCount);
    }
}