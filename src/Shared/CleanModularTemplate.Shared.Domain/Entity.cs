namespace CleanModularTemplate.Shared.Domain;

#pragma warning disable S2094
public abstract class Entity;
#pragma warning restore S2094
public abstract class Entity<TId> : Entity where TId : struct
{
  public TId Id { get; init; }

  public override bool Equals(object? obj) =>
	  obj is Entity<TId> entity && Id.Equals(entity.Id);

  public override int GetHashCode() => Id.GetHashCode();
}
