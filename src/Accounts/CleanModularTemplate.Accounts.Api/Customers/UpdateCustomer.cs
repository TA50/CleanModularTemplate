using CleanModularTemplate.Accounts.Api.Customers.Shared;
using CleanModularTemplate.Accounts.Domain.Customers;
using CleanModularTemplate.Accounts.UseCases.Customers.UpdateCustomer;
using CleanModularTemplate.IAM.Contracts;
using CleanModularTemplate.Shared.Api;
using CleanModularTemplate.Shared.Constants;
using CleanModularTemplate.Shared.Messaging;
using FastEndpoints;
using FluentValidation;

namespace CleanModularTemplate.Accounts.Api.Customers;

internal static class UpdateCustomer
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
	  var command = Mappers.Map(req);
	  var result = await _sender.Send(command, ct);
	  var res = result.ToUpdateResult(customer => customer.MapToDto());

	  await Send.ResultAsync(res);
	}

	public override void Configure()
	{
	  Put("{id:guid}");
	  Roles(Role.Customer);
	  Group<CustomersGroup>();
	}
  }


  public sealed record Request
  {
	public Guid Id { get; set; }
	public string FullName { get; set; } = string.Empty;
	public List<AddressRequest> Addresses { get; set; } = [];
  }

  public sealed record AddressRequest
  {
	public Guid? Id { get; set; }
	public string Alias { get; set; } = string.Empty;
	public string District { get; set; } = string.Empty;
	public int BuildingNumber { get; set; }
	public int SecondaryNumber { get; set; }
	public string City { get; set; } = string.Empty;
	public string Street { get; set; } = string.Empty;
	public string PostalCode { get; set; } = string.Empty;
	public double Latitude { get; set; }
	public double Longitude { get; set; }
  }

  public sealed class Validator : Validator<Request>
  {
	public Validator()
	{
	  RuleFor(x => x.Id)
		  .NotEmpty()
		  .WithErrorCode(ErrorCodes.Customers.CustomerIdRequired)
		  .WithMessage("Customer ID is required.");

	  RuleFor(x => x.FullName)
		  .ValidFullName();

	  RuleFor(x => x.Addresses)
		  .ForEach(x => x.SetValidator(new AddressValidator()));
	}
  }

  public sealed class AddressValidator : Validator<AddressRequest>
  {
	public AddressValidator()
	{
	  RuleFor(x => x.Alias)
		  .NotEmpty()
		  .WithErrorCode(ErrorCodes.AliasRequired)
		  .WithMessage("Alias is required.")
		  .MaximumLength(CustomerConstants.MaxAliasLength)
		  .WithErrorCode(ErrorCodes.AliasTooLong)
		  .WithMessage($"Alias cannot be longer than {CustomerConstants.MaxAliasLength} characters.");

	  RuleFor(x => x.Street)
		  .NotEmpty()
		  .WithErrorCode(ErrorCodes.Customers.AddressStreetRequired)
		  .WithMessage("Street is required.")
		  .MaximumLength(CustomerConstants.MaxStreetLength)
		  .WithErrorCode(ErrorCodes.Customers.AddressStreetTooLong)
		  .WithMessage($"Street cannot be longer than {CustomerConstants.MaxStreetLength} characters.");

	  RuleFor(x => x.City)
		  .NotEmpty()
		  .WithErrorCode(ErrorCodes.Customers.AddressCityRequired)
		  .WithMessage("City is required.")
		  .MaximumLength(CustomerConstants.MaxCityLength)
		  .WithErrorCode(ErrorCodes.Customers.AddressCityTooLong)
		  .WithMessage($"City cannot be longer than {CustomerConstants.MaxCityLength} characters.");

	  RuleFor(x => x.District)
		  .NotEmpty()
		  .WithErrorCode(ErrorCodes.Customers.AddressDistrictRequired)
		  .WithMessage("District is required.")
		  .MaximumLength(CustomerConstants.MaxStateLength)
		  .WithErrorCode(ErrorCodes.Customers.AddressDistrictTooLong)
		  .WithMessage($"District cannot be longer than {CustomerConstants.MaxStateLength} characters.");

	  RuleFor(x => x.PostalCode)
		  .NotEmpty()
		  .WithErrorCode(ErrorCodes.Customers.AddressPostalCodeRequired)
		  .WithMessage("Postal code is required.")
		  .Matches(RegexPatterns.PostalCode)
		  .WithErrorCode(ErrorCodes.Customers.AddressPostalCodeInvalid)
		  .WithMessage("Invalid postal code format.")
		  .MaximumLength(CustomerConstants.MaxPostalCodeLength)
		  .WithErrorCode(ErrorCodes.Customers.AddressPostalCodeTooLong)
		  .WithMessage($"Postal code cannot be longer than {CustomerConstants.MaxPostalCodeLength} characters.");

	  RuleFor(x => x.Latitude).ValidLatitude();
	  RuleFor(x => x.Longitude).ValidLongitude();
	}
  }

  public sealed class Summary : Summary<Endpoint>
  {
	public Summary()
	{
	  Summary = "Update a customer's profile";
	  Description = $"""
			               Updates a customer's profile, including their full name and addresses. This can only be done by the customer themselves.

			               ### Error Codes
			               | Error Code | Meaning |
			               | :--- | :--- |
			               | `{ErrorCodes.Customers.CustomerIdRequired}` | Customer ID is required. |
			               | `{ErrorCodes.RequiredFullName}` | Full name is required. |
			               | `{ErrorCodes.FullNameTooLong}` | Full name is too long. |
			               | `{ErrorCodes.AliasRequired}` | Address alias is required. |
			               | `{ErrorCodes.AliasTooLong}` | Address alias is too long. |
			               | `{ErrorCodes.Customers.AddressStreetRequired}` | Address street is required. |
			               | `{ErrorCodes.Customers.AddressStreetTooLong}` | Address street is too long. |
			               | `{ErrorCodes.Customers.AddressCityRequired}` | Address city is required. |
			               | `{ErrorCodes.Customers.AddressCityTooLong}` | Address city is too long. |
			               | `{ErrorCodes.Customers.AddressDistrictRequired}` | Address district is required. |
			               | `{ErrorCodes.Customers.AddressDistrictTooLong}` | Address district is too long. |
			               | `{ErrorCodes.Customers.AddressPostalCodeRequired}` | Address postal code is required. |
			               | `{ErrorCodes.Customers.AddressPostalCodeInvalid}` | Address postal code is invalid. |
			               | `{ErrorCodes.Customers.AddressPostalCodeTooLong}` | Address postal code is too long. |
			               | `{ErrorCodes.InvalidLatitude}` | Latitude is invalid. |
			               | `{ErrorCodes.InvalidLongitude}` | Longitude is invalid. |
			               """;
	  ExampleRequest = new Request
	  {
		Id = Guid.NewGuid(),
		FullName = "John Doe",
		Addresses =
		  [
			  new AddressRequest
					{
						Id = Guid.NewGuid(),
						Alias = "Home",
						District = "Riyadh",
						BuildingNumber = 123,
						SecondaryNumber = 456,
						City = "Riyadh",
						Street = "King Fahd Road",
						PostalCode = "12345",
						Latitude = 24.7136,
						Longitude = 46.6753
					}
		  ]
	  };
	  Responses[200] = "Customer profile updated successfully.";
	  Responses[400] = "Invalid input. See error response for details.";
	  Responses[401] = "Unauthorized.";
	  Responses[403] = "Forbidden. You do not have permission to update this customer's profile.";
	  Responses[404] = "Customer not found.";
	}
  }

  public static class Mappers
  {
	public static UpdateCustomerCommand Map(Request req)
	{
	  return new UpdateCustomerCommand
	  {
		FullName = req.FullName,
		Id = req.Id,
		Addresses = req.Addresses.Select(Map).ToList()
	  };
	}

	public static UpdateAddressCommand Map(AddressRequest address)
	{
	  return new UpdateAddressCommand
	  {
		Id = address.Id,
		BuildingNumber = address.BuildingNumber,
		SecondaryNumber = address.SecondaryNumber,
		Alias = address.Alias,
		District = address.District,
		City = address.City,
		Street = address.Street,
		PostalCode = address.PostalCode,
		Latitude = address.Latitude,
		Longitude = address.Longitude
	  };
	}
  }
}
