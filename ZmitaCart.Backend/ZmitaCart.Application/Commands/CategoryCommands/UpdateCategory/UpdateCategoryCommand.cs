using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.CategoryCommands.UpdateCategory;

public record UpdateCategoryCommand : IRequest<Result<int>>
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public int? ParentId { get; init; }
    public string? IconName { get; init; }
}