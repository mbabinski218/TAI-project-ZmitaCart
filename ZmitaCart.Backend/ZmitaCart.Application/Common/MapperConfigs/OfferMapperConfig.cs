using Mapster;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.Common.MapperConfigs;

public class OfferMapperConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.ForType<Offer, OfferInfoDto>()
			.Map(dest => dest.City, src => src.User.Address.City)
			.Map(dest => dest.ImageName, src => !src.Pictures.Any()
				? null
				: src.Pictures.OrderBy(p => p.CreatedAt).First().Name)
			.Map(dest => dest.AuthorEmail, src => src.User.Email)
			.Map(dest => dest.AuthorName, src => src.User.FirstName);

		config.ForType<Offer, OfferDto>()
			.Map(dest => dest.Address, src => src.User.Address)
			.Map(dest => dest.PicturesNames, src => !src.Pictures.Any()
				? null
				: src.Pictures.Select(p => p.Name));

		config.ForType<Bought, BoughtOfferDto>()
			.Map(dest => dest.Offer.Id, src => src.Offer.Id)
			.Map(dest => dest.Offer.Price, src => src.Offer.Price)
			.Map(dest => dest.Offer.Title, src => src.Offer.Title)
			.Map(dest => dest.Offer.City, src => src.User.Address.City)
			.Map(dest => dest.Offer.Quantity, src => src.Offer.Quantity)
			.Map(dest => dest.Offer.IsAvailable, src => src.Offer.IsAvailable)
			.Map(dest => dest.Offer.ImageName, src => !src.Offer.Pictures.Any()
				? null
				: src.Offer.Pictures.OrderBy(p => p.CreatedAt).First().Name)
			.Map(dest => dest.Offer.AuthorEmail, src => src.User.Email)
			.Map(dest => dest.Offer.AuthorName, src => src.User.FirstName)
			.Map(dest => dest.BoughtQuantity, src => src.Quantity);
		
		config.ForType<Offer, OfferDataDto>()
			.Map(dest => dest.PicturesNames, src => !src.Pictures.Any()
				? null
				: src.Pictures.Select(p => p.Name));
	}
}