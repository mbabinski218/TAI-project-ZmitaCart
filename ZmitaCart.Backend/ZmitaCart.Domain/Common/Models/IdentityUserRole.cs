using Microsoft.AspNetCore.Identity;

namespace ZmitaCart.Domain.Common.Models;

public sealed class IdentityUserRole : IdentityRole<int>
{
	public IdentityUserRole(string name) : base(name)
	{
	}
}