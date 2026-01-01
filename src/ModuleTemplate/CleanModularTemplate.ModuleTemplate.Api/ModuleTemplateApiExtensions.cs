using System.Reflection;
using CleanModularTemplate.ModuleTemplate.Infrastructure;
using CleanModularTemplate.ModuleTemplate.UseCases;
using CleanModularTemplate.Shared.Api;
using Microsoft.Extensions.Hosting;

namespace CleanModularTemplate.ModuleTemplate.Api;

public static class ModuleTemplateApiExtensions
{
  public static IHostApplicationBuilder AddModuleTemplate(this IHostApplicationBuilder builder, List<Assembly> assemblies)
  {
	ArgumentNullException.ThrowIfNull(builder);
	ArgumentNullException.ThrowIfNull(assemblies);
	builder.AddModuleTemplateInfrastructure();
	builder.Services.AddModuleTemplateUseCases(assemblies);
	return builder;
  }


  public static readonly SwaggerTagGroups ModuleTemplateTagGroups = new("ModuleTemplate", new[] { "Todo" });
}
