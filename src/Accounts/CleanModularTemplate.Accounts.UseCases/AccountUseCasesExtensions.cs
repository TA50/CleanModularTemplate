using System.Reflection;
using CleanModularTemplate.Accounts.UseCases.Customers.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace CleanModularTemplate.Accounts.UseCases;

public static class AccountUseCasesExtensions
{
  public static void AddAccountUseCases(this IServiceCollection services, List<Assembly> assemblies)
  {
	assemblies.Add(typeof(AccountUseCasesExtensions).Assembly);
	services.AddSingleton<CustomerMetrics>();
	services.AddOpenTelemetry()
		.WithTracing(tracing => tracing
			.AddSource(AccountDiagnostics.ActivitySourceName, AccountDiagnostics.ActivitySourceVersion)
		)
		.WithMetrics(metrics =>
		{
		  metrics
				  .AddMeter(AccountDiagnostics.CustomersMeterName);
		});
  }
}
