using Ardalis.Result;
using CleanModularTemplate.Accounts.Domain.Customers.Entities;
using CleanModularTemplate.Shared.Messaging;

namespace CleanModularTemplate.Accounts.UseCases.Customers.UpdateCustomer;

public record UpdateCustomerCommand : ICommand<Result<Customer>>
{
  public Guid Id { get; init; } = Guid.Empty;
  public string FullName { get; init; } = string.Empty;
  public List<UpdateAddressCommand> Addresses { get; init; } = [];
}

public record UpdateAddressCommand
{
  public Guid? Id { get; set; }
  public string Alias { get; init; } = string.Empty;
  public string Street { get; init; } = string.Empty;
  public string City { get; init; } = string.Empty;
  public string District { get; init; } = string.Empty;
  public string PostalCode { get; init; } = string.Empty;
  public int BuildingNumber { get; set; }
  public int SecondaryNumber { get; set; }
  public double Latitude { get; init; }
  public double Longitude { get; init; }
}
