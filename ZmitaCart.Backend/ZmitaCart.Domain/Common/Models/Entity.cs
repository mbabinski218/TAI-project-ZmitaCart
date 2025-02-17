﻿namespace ZmitaCart.Domain.Common.Models;

public abstract class Entity<TId> : IEntity, IEquatable<Entity<TId>>
	where TId: notnull
{
	public TId Id { get; init; } = default!;
	public DateTimeOffset CreatedAt { get; set; }
	public DateTimeOffset? UpdatedAt { get; set; }

	public override bool Equals(object? obj)
	{
		return obj is Entity<TId> entity && Id.Equals(entity.Id);
	}
	
	public bool Equals(Entity<TId>? other)
	{
		return Equals((object?)other);
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}
	
	public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
	{
		return Equals(left, right);
	}
	
	public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
	{
		return !Equals(left, right);
	}
}