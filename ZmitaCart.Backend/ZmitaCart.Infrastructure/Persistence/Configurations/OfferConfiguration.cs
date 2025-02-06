using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Common.Types;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.Property(o => o.Title).IsRequired().HasMaxLength(Constants.nameLength);
        builder.Property(o => o.Description).IsRequired().HasMaxLength(Constants.descriptionLength);
        builder.Property(o => o.Condition).IsRequired().HasMaxLength(Constants.shortNameLength);
        builder.Property(o => o.Quantity).IsRequired();
        builder.Property(o => o.IsAvailable).IsRequired();
        builder.Property(o => o.CreatedAt).IsRequired();
        builder.Property(o => o.Price).IsRequired().HasPrecision(Constants.pricePrecision, Constants.priceScale);
        
        builder.HasOne(o => o.User)
            .WithMany(u => u.Offers)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}