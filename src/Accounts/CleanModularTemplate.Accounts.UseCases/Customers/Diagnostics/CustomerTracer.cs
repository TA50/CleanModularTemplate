using System.Diagnostics;

namespace CleanModularTemplate.Accounts.UseCases.Customers.Diagnostics;

internal static class CustomerTracer
{
  private static readonly ActivitySource _activitySource = new(AccountDiagnostics.ActivitySourceName);

  internal static Activity? StartActivity(string name)
  {
	return _activitySource.StartActivity(name);
  }

  internal static void SetCustomerTags(Guid id, Guid userId, string? fullName = null)
  {
	SetUserIdTag(userId);
	SetCustomerIdTag(id);
	SetFullNameTag(fullName);
  }

  internal static void SetUserIdTag(Guid userId)
  {
	Activity.Current?.SetTag("customer.userId", userId);
  }

  internal static void SetCustomerIdTag(Guid id)
  {
	Activity.Current?.SetTag("customer.id", id);
  }

  internal static void SetFullNameTag(string? fullName = null)
  {
	if (!string.IsNullOrEmpty(fullName))
	{
	  Activity.Current?.SetTag("customer.fullName", fullName);
	}
  }
}
