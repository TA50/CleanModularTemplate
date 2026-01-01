using Ardalis.Result;
using CleanModularTemplate.Accounts.Domain.Customers.Entities;
using CleanModularTemplate.Shared.Messaging;

namespace CleanModularTemplate.Accounts.UseCases.Customers.CreateCustomer;

public record CreateCustomerCommand : ICommand<Result<Customer>>
{
  public string FullName { get; init; } = string.Empty;
  public Guid UserId { get; set; }
}
