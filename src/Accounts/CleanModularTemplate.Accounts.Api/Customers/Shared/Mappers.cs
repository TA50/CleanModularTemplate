using CleanModularTemplate.Accounts.Domain.Customers.Entities;

namespace CleanModularTemplate.Accounts.Api.Customers.Shared;


internal static class Mappers
{
  public static CustomerDto MapToDto(this Customer entity)
  {
	return new CustomerDto
	{
	  UserId = entity.UserId,
	  Id = entity.Id,
	  FullName = entity.FullName,
	  Addresses = entity.Addresses.Select(address => address.MapToDto()).ToList(),
	};
  }
  internal static AddressDto MapToDto(this Address entity)
  {
	return new AddressDto
	{
	  Id = entity.Id,
	  City = entity.PostalDetails.City,
	  BuildingNumber = entity.PostalDetails.BuildingNumber,
	  SecondaryNumber = entity.PostalDetails.SecondaryNumber,
	  Street = entity.PostalDetails.Street,
	  PostalCode = entity.PostalDetails.PostalCode,
	  District = entity.PostalDetails.District,
	  Longitude = entity.Coordinates.Longitude,
	  Latitude = entity.Coordinates.Latitude
	};
  }
}
