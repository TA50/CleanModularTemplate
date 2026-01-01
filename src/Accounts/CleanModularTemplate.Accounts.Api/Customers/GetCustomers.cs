using CleanModularTemplate.Accounts.Api.Customers.Shared;
using CleanModularTemplate.IAM.Contracts;
using FastEndpoints;

namespace CleanModularTemplate.Accounts.Api.Customers;

internal static class GetCustomers
{
  public sealed class Endpoint : EndpointWithoutRequest<IEnumerable<CustomerDto>>
  {
	public override async Task HandleAsync(CancellationToken ct)
	{
	  var customers = new[]
	  {
				new CustomerDto
				{
					Addresses = [],
					PhoneNumber = "+966500000000",
					Id = Guid.NewGuid(),
					FullName = "John Doe",
					UserId = Guid.NewGuid()
				},
			};
	  await Send.OkAsync(customers, cancellation: ct);
	}

	public override void Configure()
	{
	  Get("");
	  Group<CustomersGroup>();
	  Permissions(Permission.Customers.ReadAll);
	}
  }

  public sealed class Summary : Summary<Endpoint>
  {
	public Summary()
	{
	  Summary = "Get all customers";
	  Description = "Returns all customers.";
	  Responses[200] = "Customers found.";
	  Responses[401] = "Unauthorized.";
	  Responses[403] = "Forbidden.";
	}
  }
}
