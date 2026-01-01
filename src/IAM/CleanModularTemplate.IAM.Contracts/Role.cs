using System.Collections.ObjectModel;

namespace CleanModularTemplate.IAM.Contracts;

public sealed class Role : IEquatable<Role>
{

  public static Role SuperAdmin => new Role(nameof(SuperAdmin), [
	  Permission.Orders.ReadAll,
		Permission.Orders.Create,
		Permission.Orders.DeleteAll,
		Permission.Orders.ModifyAll,
		Permission.Riders.CreateAll,
		Permission.Riders.ReadAll,
		Permission.Riders.UpdateAll,
		Permission.Riders.DeleteAll,
		Permission.Riders.Enable,
		Permission.Riders.Disable,
		Permission.Riders.ReadLocationHistory,
		Permission.StoreAgents.CreateAll,
		Permission.StoreAgents.ReadAll,
		Permission.StoreAgents.UpdateAll,
		Permission.StoreAgents.DeleteAll,
		Permission.StoreAgents.Enable,
		Permission.StoreAgents.Disable,
		Permission.StoreAgents.ReadLocationHistory
  ]);

  public static Role Customer => new Role(nameof(Customer), [
	  Permission.Orders.ReadAll,
		Permission.Orders.Create,
		Permission.Orders.ModifyAll,
		Permission.Customers.CreateSelf,
	]);

  public static Role Rider => new Role(nameof(Rider), [
	  Permission.Orders.ReadAll,
		Permission.Orders.ModifyAll,
		Permission.Riders.AddLocationHistory,
		Permission.Riders.ReadLocationHistory,
		Permission.Riders.ReadSelf,
		Permission.Riders.UpdateSelf
  ]);

  public static Role StoreAgent => new Role(nameof(StoreAgent), [
	  Permission.StoreAgents.ReadSelf,
		Permission.StoreAgents.UpdateSelf,
		Permission.StoreAgents.AddLocationHistory,
		Permission.StoreAgents.ReadLocationHistory
  ]);


  public static ReadOnlyCollection<Role> All => [SuperAdmin, Rider, Customer, StoreAgent];

  public Role(string name, Collection<string> permissions)
  {
	ArgumentNullException.ThrowIfNull(name);
	ArgumentNullException.ThrowIfNull(permissions);
	Name = name.Trim();
	Permissions = permissions.AsReadOnly();
  }
  public string Name { get; init; }
  public ReadOnlyCollection<string> Permissions { get; init; }

  public override string ToString() => Name;

  public static implicit operator string(Role role) => role.Name;
  public override int GetHashCode()
  {
	return Name.GetHashCode(StringComparison.InvariantCulture);
  }
  public override bool Equals(object? obj)
  {
	if (obj is null)
	{
	  return false;
	}
	if (ReferenceEquals(this, obj))
	{
	  return true;
	}
	if (obj.GetType() != GetType())
	{
	  return false;
	}
	return Equals((Role)obj);
  }
  public bool Equals(Role? other)
  {
	if (other is null)
	{
	  return false;
	}
	if (ReferenceEquals(this, other))
	{
	  return true;
	}
	return Name == other.Name;
  }
}
