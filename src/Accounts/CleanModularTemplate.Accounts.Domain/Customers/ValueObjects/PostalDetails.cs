namespace CleanModularTemplate.Accounts.Domain.Customers.ValueObjects;

public record PostalDetails
{
  public string Street { get; set; } = string.Empty;
  public string City { get; set; } = string.Empty;
  public int BuildingNumber { get; set; }
  public int SecondaryNumber { get; set; }
  public string District { get; set; } = string.Empty;
  public string PostalCode { get; set; } = string.Empty;
}
