using FluentResults;
using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Dtos.CategoryDtos;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Queries.CategoryQueries.GetCategories;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesBySuperiorIdQuery, Result<IEnumerable<CategoryDto>>>,
    IRequestHandler<GetCategoriesWithChildrenBySuperiorIdQuery, Result<IEnumerable<CategoryDto>>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoriesHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetCategoriesBySuperiorIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetCategoriesBySuperiorId(request.SuperiorId, null);
    }

    public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetCategoriesWithChildrenBySuperiorIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetCategoriesBySuperiorId(request.SuperiorId, request.ChildrenCount);
    }
}