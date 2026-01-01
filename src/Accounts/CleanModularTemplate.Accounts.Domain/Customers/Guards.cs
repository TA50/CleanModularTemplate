using Ardalis.GuardClauses;
using CleanModularTemplate.Accounts.Domain.Customers.ValueObjects;
using CleanModularTemplate.Accounts.Domain.Shared;

namespace CleanModularTemplate.Accounts.Domain.Customers;

public static class Guards
{
  public static void InvalidCustomerName(this IGuardClause guardClause, string customerName)
  {
	guardClause.NullOrEmpty(customerName);
	guardClause.StringTooLong(customerName, AccountConstants.MaxNameLength);
  }

  public static void InvalidPostalDetails(this IGuardClause guardClause, PostalDetails details)
  {
	guardClause.Null(details);
	guardClause.NullOrEmpty(details.City);
	guardClause.StringTooLong(details.City, CustomerConstants.MaxCityLength);
	guardClause.NullOrEmpty(details.District);
	guardClause.StringTooLong(details.District, CustomerConstants.MaxStateLength);
	guardClause.NullOrEmpty(details.Street);
	guardClause.StringTooLong(details.Street, CustomerConstants.MaxStreetLength);
	guardClause.NullOrEmpty(details.PostalCode);
	guardClause.StringTooLong(details.PostalCode, CustomerConstants.MaxPostalCodeLength);
	guardClause.NegativeOrZero(details.BuildingNumber);
	guardClause.NegativeOrZero(details.SecondaryNumber);
  }

}
