using Microsoft.AspNetCore.Identity;

namespace ZmitaCart.Domain.Common.Models;

public class IdentityEntity<TId> : IdentityUser<TId>, IEntity
	where TId : IEquatable<TId>
{
	public DateTimeOffset CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }
}