using System.Reflection;
using CleanModularTemplate.Accounts.Domain.Shared;
using CleanModularTemplate.Accounts.Infrastructure;
using CleanModularTemplate.Accounts.UseCases;
using CleanModularTemplate.Shared.Api;
using CleanModularTemplate.Shared.Constants;
using FluentValidation;
using Microsoft.Extensions.Hosting;

namespace CleanModularTemplate.Accounts.Api;

public static class AccountsApiExtensions
{
  public static IHostApplicationBuilder AddAccounts(this IHostApplicationBuilder builder, List<Assembly> assemblies)
  {
	ArgumentNullException.ThrowIfNull(builder);
	ArgumentNullException.ThrowIfNull(assemblies);
	builder.AddAccountInfrastructure();
	builder.Services.AddAccountUseCases(assemblies);
	return builder;
  }

  public static IRuleBuilderOptions<T, string> ValidPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
	return ruleBuilder
		.NotEmpty()
		.WithMessage("PhoneNumber is required")
		.WithErrorCode(ErrorCodes.RequiredPhoneNumber)
		.Matches(RegexPatterns.PhoneNumber)
		.WithErrorCode(ErrorCodes.InvalidPhoneNumber)
		.WithMessage("Invalid phone number format");
  }

  public static IRuleBuilderOptions<T, string> ValidFullName<T>(this IRuleBuilder<T, string> ruleBuilder)
  {
	return ruleBuilder
		.NotEmpty()
		.WithMessage("FullName is required")
		.WithErrorCode(ErrorCodes.RequiredFullName)
		.MaximumLength(AccountConstants.MaxNameLength)
		.WithErrorCode(ErrorCodes.FullNameTooLong)
		.WithMessage($"Full name cannot be longer than {AccountConstants.MaxNameLength} characters.");
  }

  public static IRuleBuilderOptions<T, double> ValidLatitude<T>(this IRuleBuilder<T, double> ruleBuilder)
  {
	return ruleBuilder
		.InclusiveBetween(Coordinates.MinLatitude, Coordinates.MaxLatitude)
		.WithErrorCode(ErrorCodes.InvalidLatitude)
		.WithMessage($"Latitude must be between {Coordinates.MinLatitude} and {Coordinates.MaxLatitude}.");
  }

  public static IRuleBuilderOptions<T, double> ValidLongitude<T>(this IRuleBuilder<T, double> ruleBuilder)
  {
	return ruleBuilder
		.InclusiveBetween(Coordinates.MinLongitude, Coordinates.MaxLongitude)
		.WithErrorCode(ErrorCodes.InvalidLongitude)
		.WithMessage($"Longitude must be between {Coordinates.MinLongitude} and {Coordinates.MaxLongitude}.");
  }

  public static readonly SwaggerTagGroups AccountsTagGroups = new("Accounts", new[] { "Customers" });
}
