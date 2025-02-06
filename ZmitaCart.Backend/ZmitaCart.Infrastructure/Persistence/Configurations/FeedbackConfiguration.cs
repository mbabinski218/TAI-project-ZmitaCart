using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Common.Types;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class FeedbackConfiguration: IEntityTypeConfiguration<Feedback>
{
	public void Configure(EntityTypeBuilder<Feedback> builder)
	{
		builder.Property(f => f.RaterId).IsRequired();
		builder.Property(f => f.Rating).IsRequired();
		builder.Property(f => f.Comment).HasMaxLength(Constants.descriptionLength);
	}
}