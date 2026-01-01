namespace CleanModularTemplate.Accounts.Api;

internal static class ErrorCodes
{
  public const string Accounts = "10";
  public const string SharedCode = "00";
  public const string RequiredFullName = $"{Accounts}.{SharedCode}.10";
  public const string InvalidPhoneNumber = $"{Accounts}.{SharedCode}.20";
  public const string RequiredPhoneNumber = $"{Accounts}.{SharedCode}.30";
  public const string FullNameTooLong = $"{Accounts}.{SharedCode}.40";
  public const string InvalidLatitude = $"{Accounts}.{SharedCode}.50";
  public const string InvalidLongitude = $"{Accounts}.{SharedCode}.60";
  public const string InvalidDateRange = $"{Accounts}.{SharedCode}.70";
  public const string AliasTooLong = $"{Accounts}.{SharedCode}.80";
  public const string AliasRequired = $"{Accounts}.{SharedCode}.90";
  public const string PhoneNumberAlreadyExists = $"{Accounts}.{SharedCode}.100";

  public static class Customers
  {
	public const string CustomersCode = "10";
	public const string AddressStreetRequired = $"{Accounts}.{CustomersCode}.10";
	public const string AddressStreetTooLong = $"{Accounts}.{CustomersCode}.20";
	public const string AddressCityRequired = $"{Accounts}.{CustomersCode}.30";
	public const string AddressCityTooLong = $"{Accounts}.{CustomersCode}.40";
	public const string AddressDistrictRequired = $"{Accounts}.{CustomersCode}.50";
	public const string AddressDistrictTooLong = $"{Accounts}.{CustomersCode}.60";
	public const string AddressPostalCodeRequired = $"{Accounts}.{CustomersCode}.70";
	public const string AddressPostalCodeInvalid = $"{Accounts}.{CustomersCode}.80";
	public const string AddressPostalCodeTooLong = $"{Accounts}.{CustomersCode}.90";
	public const string CustomerIdRequired = $"{Accounts}.{CustomersCode}.100";
  }
}
