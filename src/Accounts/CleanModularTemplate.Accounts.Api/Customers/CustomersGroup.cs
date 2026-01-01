using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace CleanModularTemplate.Accounts.Api.Customers;

internal sealed class CustomersGroup : Group
{
  public CustomersGroup()
  {
	Configure("customers/v1", ep => ep
		.Description(x => x
			.WithSummary("Customers"))
	);
  }
}
