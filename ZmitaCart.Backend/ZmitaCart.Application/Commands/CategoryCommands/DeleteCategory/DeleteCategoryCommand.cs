using FluentResults;
using MediatR;

namespace ZmitaCart.Application.Commands.CategoryCommands.DeleteCategory;

public record DeleteCategoryCommand(int Id) : IRequest<Result>;