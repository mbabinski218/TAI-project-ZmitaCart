using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Common.Types;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("Users");

		builder.Property(u => u.FirstName).IsRequired().HasMaxLength(Constants.nameLength);
		builder.Property(u => u.LastName).IsRequired().HasMaxLength(Constants.nameLength);

		builder.OwnsOne(u => u.Address, addressBuilder =>
		{
			addressBuilder.Property(a => a.Country).HasMaxLength(Constants.addressLength);
			addressBuilder.Property(a => a.City).HasMaxLength(Constants.addressLength);
			addressBuilder.Property(a => a.Street).HasMaxLength(Constants.addressLength);
		});
	}
}