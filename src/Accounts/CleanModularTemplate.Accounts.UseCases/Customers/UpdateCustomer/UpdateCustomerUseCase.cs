using Ardalis.Result;
using CleanModularTemplate.Accounts.Domain.Customers.Entities;
using CleanModularTemplate.Accounts.Domain.Customers.ValueObjects;
using CleanModularTemplate.Accounts.Domain.Shared;
using CleanModularTemplate.Accounts.UseCases.Customers.Diagnostics;
using CleanModularTemplate.Shared.Messaging;

namespace CleanModularTemplate.Accounts.UseCases.Customers.UpdateCustomer;

internal sealed class UpdateCustomerUseCase : ICommandHandler<UpdateCustomerCommand, Result<Customer>>
{
  private readonly IAccountRepository<Customer> _accountRepository;

  public UpdateCustomerUseCase(IAccountRepository<Customer> accountRepository)
  {
	_accountRepository = accountRepository;
  }

  public async Task<Result<Customer>> Handle(UpdateCustomerCommand command, CancellationToken ct = default)
  {
	using var activity = CustomerTracer.StartActivity("UpdateCustomer");
	CustomerTracer.SetCustomerIdTag(command.Id);
	CustomerTracer.SetFullNameTag(command.FullName);

	var customer = await _accountRepository.GetByIdAsync(command.Id, ct);

	if (customer is null)
	{
	  return Result.NotFound();
	}

	CustomerTracer.SetUserIdTag(customer.UserId);

	try
	{
	  customer.SetFullName(command.FullName);
	  var currentAddresses = customer.Addresses.ToList();
	  var currentAddressesIds = customer.Addresses.Select(x => x.Id).ToList();
	  var existingAddresses = command.Addresses
		  .Where(addr => addr.Id is not null)
		  .Where(addr => currentAddressesIds.Contains(addr.Id!.Value));
	  var deletedAddresses = currentAddresses.ExceptBy(command.Addresses.Select(s => s.Id), addr => addr.Id);
	  var newAddresses = command.Addresses.Where(addr => !addr.Id.HasValue);


	  foreach (var addr in existingAddresses)
	  {
		var customerAddress = customer.Addresses.FirstOrDefault(x => x.Id == addr.Id);
		if (customerAddress is null) continue;
		var postal = new PostalDetails
		{
		  Street = addr.Street,
		  BuildingNumber = addr.BuildingNumber,
		  SecondaryNumber = addr.SecondaryNumber,
		  District = addr.District,
		  City = addr.City,
		  PostalCode = addr.PostalCode
		};
		var coords = new Coordinates
		{
		  Latitude = addr.Latitude,
		  Longitude = addr.Longitude
		};
		customerAddress.UpdateCoordinates(coords);
		customerAddress.UpdatePostalDetails(postal);
		customerAddress.SetAlias(addr.Alias);
	  }
	  foreach (var addr in newAddresses)
	  {
		var postal = new PostalDetails
		{
		  Street = addr.Street,
		  BuildingNumber = addr.BuildingNumber,
		  SecondaryNumber = addr.SecondaryNumber,
		  District = addr.District,
		  City = addr.City,
		  PostalCode = addr.PostalCode
		};
		var coords = new Coordinates
		{
		  Latitude = addr.Latitude,
		  Longitude = addr.Longitude
		};
		var address = new Address(postal, coords);
		address.SetAlias(addr.Alias);
		customer.AddAddress(address);
	  }
	  foreach (var addr in deletedAddresses)
	  {
		customer.RemoveAddress(addr);
	  }
	  customer.MarkAsUpdated();
	  await _accountRepository.UpdateAsync(customer, ct);
	  return Result.Success(customer);
	}
	catch (ArgumentException ex)
	{
	  return Result.Invalid(new ValidationError(ex.Message));
	}
  }
}
