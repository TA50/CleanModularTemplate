using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CleanModularTemplate.ModuleTemplate.UseCases;

public static class ModuleTemplateUseCasesExtensions
{
  public static void AddModuleTemplateUseCases(this IServiceCollection services, List<Assembly> assemblies)
  {
	assemblies.Add(typeof(ModuleTemplateUseCasesExtensions).Assembly);
	services.AddOpenTelemetry()
		.WithTracing(tracing => tracing
			.AddSource(ModuleTemplateDiagnostics.ActivitySourceName, ModuleTemplateDiagnostics.ActivitySourceVersion)
		)
		.WithMetrics(metrics =>
		{
		  metrics
				  .AddMeter(ModuleTemplateDiagnostics.CustomersMeterName);
		});
  }
}
