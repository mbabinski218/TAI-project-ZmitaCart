using Mapster;
using ZmitaCart.Application.Dtos.CategoryDtos;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.Common.MapperConfigs;

public class CategoryMapperConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<Category, CategoryDto>()
			.Map(dest => dest.Children, src => !src.Children.Any()
				? null
				: src.Children.Adapt<ICollection<CategoryDto>>());
	}
}