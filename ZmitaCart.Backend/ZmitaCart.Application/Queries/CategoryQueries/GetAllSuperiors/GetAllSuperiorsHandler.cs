using FluentResults;
using MapsterMapper;
using MediatR;
using ZmitaCart.Application.Dtos.CategoryDtos;
using ZmitaCart.Application.Interfaces.Repositories;

namespace ZmitaCart.Application.Queries.CategoryQueries.GetAllSuperiors;

public class GetAllSuperiorsHandler : IRequestHandler<GetAllSuperiorsQuery, Result<IEnumerable<SuperiorCategoryDto>>>
{
	private readonly ICategoryRepository _categoryRepository;
	private readonly IMapper _mapper;

	public GetAllSuperiorsHandler(ICategoryRepository categoryRepository, IMapper mapper)
	{
		_categoryRepository = categoryRepository;
		_mapper = mapper;
	}

	public async Task<Result<IEnumerable<SuperiorCategoryDto>>> Handle(GetAllSuperiorsQuery request, CancellationToken cancellationToken)
	{
		return await _categoryRepository.GetAllSuperiors();
	}
}