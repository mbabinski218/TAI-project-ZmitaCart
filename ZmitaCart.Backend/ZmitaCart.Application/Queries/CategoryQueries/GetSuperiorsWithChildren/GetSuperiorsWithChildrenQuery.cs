using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.CategoryDtos;

namespace ZmitaCart.Application.Queries.CategoryQueries.GetSuperiorsWithChildren;

public record GetSuperiorsWithChildrenQuery : IRequest<Result<IEnumerable<CategoryDto>>>
{
    public int? ChildrenCount { get; init; }
}