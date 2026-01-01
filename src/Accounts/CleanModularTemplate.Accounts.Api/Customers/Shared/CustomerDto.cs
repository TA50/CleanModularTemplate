namespace CleanModularTemplate.Accounts.Api.Customers.Shared;

internal sealed record CustomerDto
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public string FullName { get; set; } = string.Empty;
  public string PhoneNumber { get; set; } = string.Empty;
  public List<AddressDto> Addresses { get; set; } = [];
}

