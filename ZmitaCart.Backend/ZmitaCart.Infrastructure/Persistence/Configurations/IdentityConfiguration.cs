using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZmitaCart.Domain.Common.Models;

namespace ZmitaCart.Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<IdentityUserRole>
{
	public void Configure(EntityTypeBuilder<IdentityUserRole> builder)
	{
		builder.ToTable("Roles");
	}
}

public sealed class UserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<int>>
{
	public void Configure(EntityTypeBuilder<IdentityUserClaim<int>> builder)
	{
		builder.ToTable("UserClaims");
	}
}

public sealed class UserLoginsConfiguration : IEntityTypeConfiguration<IdentityUserLogin<int>>
{
	public void Configure(EntityTypeBuilder<IdentityUserLogin<int>> builder)
	{
		builder.ToTable("UserLogins");
	}
}

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<int>>
{
	public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
	{
		builder.ToTable("UserRoles");
	}
}

public sealed class UserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<int>>
{
	public void Configure(EntityTypeBuilder<IdentityUserToken<int>> builder)
	{
		builder.ToTable("UserTokens");
	}
}