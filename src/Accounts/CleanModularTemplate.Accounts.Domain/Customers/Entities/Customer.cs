using Ardalis.GuardClauses;
using CleanModularTemplate.Accounts.Domain.Customers.Events;
using CleanModularTemplate.Shared.Domain;

namespace CleanModularTemplate.Accounts.Domain.Customers.Entities;

public sealed class Customer : Account
{
  private readonly List<Address> _addresses;
  public Customer(Guid userId, string fullName) : base(userId, userId)
  {
	FullName = null!;
	Id = Guid.CreateVersion7();
	UserId = userId;
	SetFullName(fullName);
	_addresses = [];
  }
  public Guid? DefaultAddressId { get; set; }

  public IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

  public void SetDefaultAddress(Address address)
  {
	DefaultAddressId = address.Id;
  }
  public void SetFullName(string fullName)
  {
	Guard.Against.InvalidCustomerName(fullName);
	FullName = fullName;
	MarkAsUpdated();
  }
  public void AddAddress(Address address)
  {
	_addresses.Add(address);
	MarkAsUpdated();
  }
  public void RemoveAddress(Address address)
  {
	_addresses.Remove(address);
	MarkAsUpdated();
  }

  public void MarkAsUpdated()
  {
	ClearDomainEvents<CustomerUpdatedDomainEvent>();
	UpdatedAt = DateTimeOffset.UtcNow;
	UpdatedBy = UserId;

	AddDomainEvent(new CustomerUpdatedDomainEvent(this));
  }


#pragma warning disable CS8618 // Enable EF Core
  private Customer() : base()
#pragma warning restore CS8618
  {
  }


}
