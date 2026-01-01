using CleanModularTemplate.Accounts.Api.Customers.Shared;
using CleanModularTemplate.Accounts.UseCases.Customers.GetCustomer;
using CleanModularTemplate.IAM.Contracts;
using CleanModularTemplate.Shared.Api;
using CleanModularTemplate.Shared.Messaging;
using FastEndpoints;
using FluentValidation;

namespace CleanModularTemplate.Accounts.Api.Customers;

internal static class GetCustomerById
{
  public sealed class Endpoint : Endpoint<Request, CustomerDto>
  {
	private readonly ISender _sender;

	public Endpoint(ISender sender)
	{
	  _sender = sender;
	}

	public override async Task HandleAsync(Request req, CancellationToken ct)
	{
	  var query = new GetCustomerByIdQuery(req.Id);
	  var result = await _sender.Send(query, ct);

	  var res = result.ToGetByIdResult(r => r.MapToDto());
	  await Send.ResultAsync(res);
	}

	public override void Configure()
	{
	  Get("{Id:guid}");
	  Roles(Role.Customer);
	  Group<CustomersGroup>();
	}
  }

  public sealed record Request
  {
	public Guid Id { get; set; }
  }

  public sealed class Validator : Validator<Request>
  {
	public Validator()
	{
	  RuleFor(x => x.Id)
		  .NotEmpty()
		  .WithErrorCode(ErrorCodes.Customers.CustomerIdRequired)
		  .WithMessage("Customer ID is required.");
	}
  }

  public sealed class Summary : Summary<Endpoint>
  {
	public Summary()
	{
	  Summary = "Get customer by id";
	  Description = "Retrieves a customer by their unique identifier.";
	  Responses[200] = "Customer found.";
	  Responses[401] = "Unauthorized.";
	  Responses[403] = "Forbidden.";
	  Responses[404] = "Customer not found.";
	}
  }
}
