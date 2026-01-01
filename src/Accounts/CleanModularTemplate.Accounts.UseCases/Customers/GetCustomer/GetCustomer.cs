using Ardalis.Result;
using CleanModularTemplate.Accounts.Domain.Customers.Entities;
using CleanModularTemplate.Accounts.Domain.Customers.Specifications;
using CleanModularTemplate.Accounts.UseCases.Customers.Diagnostics;
using CleanModularTemplate.Shared.Messaging;

namespace CleanModularTemplate.Accounts.UseCases.Customers.GetCustomer;

internal sealed class GetCustomerUseCase :
	ICommandHandler<GetCustomerByUserIdQuery, Result<Customer>>,
	ICommandHandler<GetCustomerByIdQuery, Result<Customer>>
{
  private readonly IAccountRepository<Customer> _repository;
  public GetCustomerUseCase(IAccountRepository<Customer> repository)
  {
	_repository = repository;
  }

  public async Task<Result<Customer>> Handle(GetCustomerByUserIdQuery command, CancellationToken ct = default)
  {
	using var activity = CustomerTracer.StartActivity("GetCustomerByUserId");
	CustomerTracer.SetUserIdTag(command.UserId);
	var customer = await _repository.FirstOrDefaultAsync(new GetCustomerByUserIdSpec(command.UserId), ct);
	if (customer is null)
	{
	  return Result.NotFound();
	}
	CustomerTracer.SetCustomerIdTag(customer.Id);
	return customer;
  }
  public async Task<Result<Customer>> Handle(GetCustomerByIdQuery command, CancellationToken ct = default)
  {
	using var activity = CustomerTracer.StartActivity("GetCustomerById");
	CustomerTracer.SetCustomerIdTag(command.CustomerId);
	var customer = await _repository.GetByIdAsync(command.CustomerId, ct);
	if (customer is null)
	{
	  return Result.NotFound();
	}
	CustomerTracer.SetUserIdTag(customer.UserId);
	return customer;
  }
}

public record GetCustomerByUserIdQuery(Guid UserId) : ICommand<Result<Customer>>;

public record GetCustomerByIdQuery(Guid CustomerId) : ICommand<Result<Customer>>;
