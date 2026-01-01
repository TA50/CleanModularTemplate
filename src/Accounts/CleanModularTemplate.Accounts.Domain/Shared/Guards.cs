using Ardalis.GuardClauses;
using CleanModularTemplate.Accounts.Domain.Customers;

namespace CleanModularTemplate.Accounts.Domain.Shared;

public static class SharedGuards
{
  public static IGuardClause InvalidCoordinates(this IGuardClause guardClause, Coordinates coordinates)
  {
	guardClause.Null(coordinates);

	// Validate Latitude (-90 to 90)
	if (coordinates.Latitude is < Coordinates.MinLatitude or > Coordinates.MaxLongitude)
	{
	  throw new ArgumentException($"Latitude must be between -90 and 90 degrees. Found: {coordinates.Latitude}", nameof(coordinates));
	}

	if (coordinates.Longitude is < Coordinates.MinLongitude or > Coordinates.MaxLongitude)
	{
	  throw new ArgumentException($"Longitude must be between -180 and 180 degrees. Found: {coordinates.Longitude}", nameof(coordinates));
	}

	return guardClause;
  }
}
