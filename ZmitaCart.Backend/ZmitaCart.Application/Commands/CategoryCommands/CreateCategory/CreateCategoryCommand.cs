using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.CategoryCommands.CreateCategory;

public record CreateCategoryCommand : IRequest<Result<int>>
{
    public string Name { get; init; } = null!;
    public int? ParentId { get; init; }
    public string? IconName { get; init; }
}