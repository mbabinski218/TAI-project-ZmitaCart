using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Common.Types;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class PictureConfiguration : IEntityTypeConfiguration<Picture>
{
	public void Configure(EntityTypeBuilder<Picture> builder)
	{
		builder.Property(p => p.Name).IsRequired().HasMaxLength(Constants.nameLength);
	}
}