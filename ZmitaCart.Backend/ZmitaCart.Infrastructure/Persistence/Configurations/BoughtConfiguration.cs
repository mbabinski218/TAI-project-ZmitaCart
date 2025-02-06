using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Common.Types;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class BoughtConfiguration : IEntityTypeConfiguration<Bought>
{
	public void Configure(EntityTypeBuilder<Bought> builder)
	{
		builder.Property(b => b.Quantity).IsRequired();
		builder.Property(b => b.BoughtAt).IsRequired();
		builder.Property(b => b.UserId).IsRequired();
		builder.Property(b => b.OfferId).IsRequired();
		builder.Property(o => o.TotalPrice).IsRequired().HasPrecision(Constants.pricePrecision, Constants.priceScale);
	}
}