using CleanModularTemplate.Accounts.Domain.Customers.Entities;
using CleanModularTemplate.Shared.Domain;

namespace CleanModularTemplate.Accounts.Domain.Customers.Events;

public record CustomerCreatedDomainEvent(Customer Customer) : IDomainEvent
{

  public DateTimeOffset RaisedAt { get; set; } = DateTimeOffset.Now;
}
