using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class UserOfferConfiguration : IEntityTypeConfiguration<UserOffer>
{
	public void Configure(EntityTypeBuilder<UserOffer> userOfferModelBuilder)
	{
		userOfferModelBuilder.Property(uo => uo.UserId).IsRequired();
		userOfferModelBuilder.Property(uo => uo.OfferId).IsRequired();

		userOfferModelBuilder
			.HasKey(uo => new { uo.OfferId, uo.UserId });
		
		userOfferModelBuilder
			.HasOne(uo => uo.Offer)
			.WithMany(o => o.Favorites)
			.HasForeignKey(uo => uo.OfferId)
			.OnDelete(DeleteBehavior.NoAction);
		
		userOfferModelBuilder
			.HasOne(uo => uo.User)
			.WithMany(u => u.Favorites)
			.HasForeignKey(uo => uo.UserId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}