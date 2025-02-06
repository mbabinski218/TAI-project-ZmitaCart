using FluentResults;
using MediatR;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Commands.CategoryCommands.UpdateCategory;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<int>>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<int>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.Update(request.Id, request.Name, request.ParentId, request.IconName);
    }
}