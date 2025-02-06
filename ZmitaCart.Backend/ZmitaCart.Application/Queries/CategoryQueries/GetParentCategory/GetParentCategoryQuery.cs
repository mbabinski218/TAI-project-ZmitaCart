using FluentResults;
using MediatR;
using ZmitaCart.Application.Dtos.CategoryDtos;

namespace ZmitaCart.Application.Queries.CategoryQueries.GetParentCategory;

public record GetParentCategoryQuery : IRequest<Result<CategoryDto?>>
{
    public int ParentId { get; init; }
}