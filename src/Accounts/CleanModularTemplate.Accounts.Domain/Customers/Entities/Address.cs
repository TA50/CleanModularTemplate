using Ardalis.GuardClauses;
using CleanModularTemplate.Accounts.Domain.Customers.ValueObjects;
using CleanModularTemplate.Accounts.Domain.Shared;
using CleanModularTemplate.Shared.Domain;

namespace CleanModularTemplate.Accounts.Domain.Customers.Entities;

public sealed class Address : Entity<Guid>
{
  public Address(PostalDetails postalDetails, Coordinates coordinates)
  {
	Id = Guid.CreateVersion7();
	Guard.Against.InvalidPostalDetails(postalDetails);
	Guard.Against.InvalidCoordinates(coordinates);

	PostalDetails = postalDetails;
	Coordinates = coordinates;
  }
  public string Alias { get; private set; } = string.Empty;

  public PostalDetails PostalDetails { get; private set; }
  public Coordinates Coordinates { get; private set; }

  public void SetAlias(string alias)
  {
	Guard.Against.StringTooLong(alias, CustomerConstants.MaxAliasLength);
	Alias = alias;
  }
  public void UpdatePostalDetails(PostalDetails postalDetails)
  {
	Guard.Against.InvalidPostalDetails(postalDetails);
	PostalDetails = postalDetails;
  }

  public void UpdateCoordinates(Coordinates coordinates)
  {
	Guard.Against.InvalidCoordinates(coordinates);
	Coordinates = coordinates;
  }

#pragma warning disable CS8618 // Enable EF Core
  private Address()
#pragma warning restore CS8618
  {
  }
}
