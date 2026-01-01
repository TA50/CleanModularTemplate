using Microsoft.EntityFrameworkCore;

namespace CleanModularTemplate.ModuleTemplate.Infrastructure.Persistence;




internal sealed class ModuleTemplateDbContext : DbContext
{
	public const string DefaultSchema = "ModuleTemplate";
	public ModuleTemplateDbContext(DbContextOptions<ModuleTemplateDbContext> options)
		: base(options)
	{
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ModuleTemplateDbContext).Assembly);
		modelBuilder.HasDefaultSchema(DefaultSchema);
	}

}
