using CleanModularTemplate.ModuleTemplate.Infrastructure.Persistence;
using CleanModularTemplate.ModuleTemplate.UseCases;
using CleanModularTemplate.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanModularTemplate.ModuleTemplate.Infrastructure;

public static class ModuleTemplateInfrastructureExtensions
{
	public static void AddModuleTemplateInfrastructure(this IHostApplicationBuilder builder)
	{
		builder.AddDbContext<ModuleTemplateDbContext>("ModuleTemplateDb");
		builder.Services.AddScoped(typeof(IModuleTemplateRepository<>), typeof(ModuleTemplateEfRepository<>));
	}

	public static async Task InitModuleTemplatesDb(this IHost app)
	{
		ArgumentNullException.ThrowIfNull(app);
		using var scope = app.Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<ModuleTemplateDbContext>();
		await context.Database.ExecuteSqlRawAsync($"CREATE SCHEMA IF NOT EXISTS {ModuleTemplateDbContext.DefaultSchema}");
		await context.Database.MigrateAsync();
	}
}
