using FluentResults;
using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Commands.CategoryCommands.DeleteCategory;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public DeleteCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.Delete(request.Id);
    }
}