using Mapster;
using ZmitaCart.Application.Dtos.UserDtos;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.Common.MapperConfigs;

public class UserMapperConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<Feedback, FeedbackDto>()
			.Map(dest => dest.RaterName, src => src.Rater.FirstName);
	}
}