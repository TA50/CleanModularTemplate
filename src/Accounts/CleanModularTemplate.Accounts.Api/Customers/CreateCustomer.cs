using CleanModularTemplate.Accounts.Api.Customers.Shared;
using CleanModularTemplate.Accounts.UseCases.Customers.CreateCustomer;
using CleanModularTemplate.IAM.Contracts;
using CleanModularTemplate.Shared.Api;
using CleanModularTemplate.Shared.Messaging;
using FastEndpoints;

namespace CleanModularTemplate.Accounts.Api.Customers;

internal static class CreateCustomer
{
  public sealed class Endpoint : Endpoint<Request>
  {
	private readonly ISender _sender;

	public Endpoint(ISender sender)
	{
	  _sender = sender;
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
	  if (!User.TryGetUserId(out var userId))
	  {
		await Send.UnauthorizedAsync(ct);
		return;
	  }

	  var command = new CreateCustomerCommand { FullName = req.FullName, UserId = userId, };
	  var result = await _sender.Send(command, ct);

	  var res = result.ToCreatedResult(id => $"/customers/v1/{id}",
		  customer => customer.MapToDto());

	  await Send.ResultAsync(res);
	}

	public override void Configure()
	{
	  Post("");
	  Permissions(Permission.Customers.CreateSelf);
	  Group<CustomersGroup>();
	}
  }

  public sealed record Request
  {
	public string FullName { get; set; } = string.Empty;
	public string PhoneNumber { get; set; } = string.Empty;
  }

  public sealed class Validator : Validator<Request>
  {
	public Validator()
	{
	  RuleFor(x => x.FullName)
		  .ValidFullName();
	  RuleFor(x => x.PhoneNumber)
		  .ValidPhoneNumber();
	}
  }

  public sealed class Summary : Summary<Endpoint>
  {
	public Summary()
	{
	  Summary = "Create a new customer profile";
	  Description = $"""
			               create a customer profile for the currently authenticated user

			               ### Error Codes
			               | Error Code | Meaning |
			               | :--- | :--- |
			               | `{ErrorCodes.RequiredFullName}` | Full name is required. |
			               | `{ErrorCodes.FullNameTooLong}` | Full name is too long. |
			               """;
	  ExampleRequest = new Request { FullName = "John Doe", PhoneNumber = "+966500000000" };
	  Responses[201] = "Customer profile created successfully.";
	  Responses[400] = "Invalid input or customer already exists.";
	  Responses[401] = "Unauthorized. User must be logged in.";
	  Responses[403] = "Forbidden. You do not have permission to create a customer profile.";
	}
  }
}
