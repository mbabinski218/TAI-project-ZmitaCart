using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.CategoryDtos;

namespace ZmitaCart.Application.Queries.CategoryQueries.GetCategories;

public record GetCategoriesWithChildrenBySuperiorIdQuery : IRequest<Result<IEnumerable<CategoryDto>>>
{
    public int SuperiorId { get; init; }
    public int ChildrenCount { get; init; }
}

public record GetCategoriesBySuperiorIdQuery : IRequest<Result<IEnumerable<CategoryDto>>>
{
    public int SuperiorId { get; init; }
}