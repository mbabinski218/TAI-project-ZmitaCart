using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Common.Types;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.Property(c => c.Name).IsRequired().HasMaxLength(Constants.nameLength);
		builder.Property(c => c.IconName).HasMaxLength(Constants.nameLength);
		
		builder.HasOne(c => c.Parent)
			.WithMany(c => c.Children)
			.HasForeignKey(c => c.ParentId);
	}
}