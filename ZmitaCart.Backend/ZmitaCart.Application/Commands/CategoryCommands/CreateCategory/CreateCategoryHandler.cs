using FluentResults;
using MediatR;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Commands.CategoryCommands.CreateCategory;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<int>>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.Create(request.Name, request.ParentId, request.IconName);
    }
}