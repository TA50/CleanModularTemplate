using CleanModularTemplate.Accounts.Api.Customers.Shared;
using CleanModularTemplate.Accounts.UseCases.Customers.GetCustomer;
using CleanModularTemplate.IAM.Contracts;
using CleanModularTemplate.Shared.Api;
using CleanModularTemplate.Shared.Messaging;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CleanModularTemplate.Accounts.Api.Customers;

internal static class GetMyProfile
{
  public sealed class Endpoint : EndpointWithoutRequest
  {
	private readonly ISender _sender;

	public Endpoint(ISender sender)
	{
	  _sender = sender;
	}

	public override async Task HandleAsync(CancellationToken ct)
	{
	  if (!User.TryGetUserId(out var userId))
	  {
		await Send.UnauthorizedAsync(ct);
		return;
	  }

	  var query = new GetCustomerByUserIdQuery(userId);
	  var result = await _sender.Send(query, ct);
	  var res = result.ToGetByIdResult(r => r.MapToDto());
	  await Send.ResultAsync(res);
	}

	public override void Configure()
	{
	  Get("me");
	  Roles(Role.Customer);
	  Group<CustomersGroup>();
	}
  }

  public sealed class Summary : Summary<Endpoint>
  {
	public Summary()
	{
	  Summary = "Get my customer profile";
	  Description = "Retrieves the profile of the currently authenticated customer.";
	  Responses[200] = "Customer profile found.";
	  Responses[404] = "Customer not found.";
	}
  }
}
