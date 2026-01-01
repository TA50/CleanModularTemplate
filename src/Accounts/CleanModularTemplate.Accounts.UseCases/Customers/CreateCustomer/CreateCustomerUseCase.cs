using Ardalis.Result;
using CleanModularTemplate.Accounts.Domain.Customers.Entities;
using CleanModularTemplate.Accounts.Domain.Customers.Events;
using CleanModularTemplate.Accounts.Domain.Customers.Specifications;
using CleanModularTemplate.Accounts.UseCases.Customers.Diagnostics;
using CleanModularTemplate.Shared.Messaging;

namespace CleanModularTemplate.Accounts.UseCases.Customers.CreateCustomer;

internal sealed class CreateCustomerUseCase : ICommandHandler<CreateCustomerCommand, Result<Customer>>
{
  private readonly IAccountRepository<Customer> _accountRepository;
  private readonly CustomerMetrics _metrics;
  public CreateCustomerUseCase(IAccountRepository<Customer> accountRepository, CustomerMetrics metrics)
  {
	_accountRepository = accountRepository;
	_metrics = metrics;
  }

  public async Task<Result<Customer>> Handle(CreateCustomerCommand command, CancellationToken ct = default)
  {
	using var activity = CustomerTracer.StartActivity("CreateCustomer");
	CustomerTracer.SetUserIdTag(command.UserId);
	CustomerTracer.SetFullNameTag(command.FullName);

	try
	{
	  var existingCustomer = await _accountRepository.FirstOrDefaultAsync(new GetCustomerByUserIdSpec(command.UserId), ct);
	  if (existingCustomer is not null)
	  {
		existingCustomer.SetFullName(command.FullName);
		await _accountRepository.UpdateAsync(existingCustomer, ct);
		return existingCustomer;
	  }
	  var customer = new Customer(command.UserId, command.FullName);
	  customer.AddDomainEvent(new CustomerCreatedDomainEvent(customer));
	  await _accountRepository.AddAsync(customer, ct);
	  _metrics.CustomerCreated();
	  return customer;
	}
	catch (ArgumentException ex)
	{
	  return Result.Invalid(new ValidationError(ex.Message));
	}
  }
}
