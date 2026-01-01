namespace CleanModularTemplate.IAM.Contracts;

public static class Permission
{
  public static class Orders
  {
	public const string ReadAll = "Permissions.Orders.ReadAll";
	public const string Create = "Permissions.Orders.Create";
	public const string ModifyAll = "Permissions.Orders.ModifyAll";
	public const string DeleteAll = "Permissions.Orders.DeleteAll";
  }

  public static class Riders
  {
	public const string CreateAll = "Permissions.Riders.CreateAll";
	public const string ReadAll = "Permissions.Riders.ReadAll";
	public const string ReadSelf = "Permissions.Riders.ReadSelf";
	public const string UpdateAll = "Permissions.Riders.UpdateAll";
	public const string UpdateSelf = "Permissions.Riders.UpdateSelf";
	public const string DeleteAll = "Permissions.Riders.DeleteAll";
	public const string Enable = "Permissions.Riders.Enable";
	public const string Disable = "Permissions.Riders.Disable";
	public const string AddLocationHistory = "Permissions.Riders.AddLocationHistory";
	public const string ReadLocationHistory = "Permissions.Riders.ReadLocationHistory";
  }

  public static class StoreAgents
  {
	public const string CreateAll = "Permissions.StoreAgents.CreateAll";
	public const string ReadAll = "Permissions.StoreAgents.ReadAll";
	public const string ReadSelf = "Permissions.StoreAgents.ReadSelf";
	public const string UpdateAll = "Permissions.StoreAgents.UpdateAll";
	public const string UpdateSelf = "Permissions.StoreAgents.UpdateSelf";
	public const string DeleteAll = "Permissions.StoreAgents.DeleteAll";
	public const string Enable = "Permissions.StoreAgents.Enable";
	public const string Disable = "Permissions.StoreAgents.Disable";
	public const string AddLocationHistory = "Permissions.StoreAgents.AddLocationHistory";
	public const string ReadLocationHistory = "Permissions.StoreAgents.ReadLocationHistory";
  }

  public static class Administrators
  {
	public const string CreateAll = "Permissions.Administrators.CreateAll";
	public const string GetAll = "Permissions.Administrators.GetAll";
	public const string Get = "Permissions.Administrators.Get";
	public const string UpdateAll = "Permissions.Administrators.UpdateAll";
	public const string DeleteAll = "Permissions.Administrators.DeleteAll";
  }

  public static class Customers
  {
	public const string CreateSelf = "Permissions.Customers.CreateSelf";
	public const string ReadAll = "Permissions.Customers.ReadAll";
	public const string DeleteAll = "Permissions.Customers.DeleteAll";
  }

  public static class Shifts
  {
	public const string ReadAll = "Permissions.Shifts.ReadAll";
	public const string CreateAll = "Permissions.Shifts.CreateAll";
	public const string UpdateAll = "Permissions.Shifts.UpdateAll";
	public const string DeleteAll = "Permissions.Shifts.DeleteAll";
  }
}
