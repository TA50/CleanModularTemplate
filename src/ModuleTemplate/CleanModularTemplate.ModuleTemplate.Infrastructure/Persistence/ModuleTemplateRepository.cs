using CleanModularTemplate.ModuleTemplate.UseCases;
using CleanModularTemplate.Shared.Domain;
using CleanModularTemplate.Shared.Infrastructure.Persistence;

namespace CleanModularTemplate.ModuleTemplate.Infrastructure.Persistence;

internal sealed class ModuleTemplateEfRepository<TEntity>(ModuleTemplateDbContext dbContext)
	: GenericEfRepository<TEntity, ModuleTemplateDbContext>(dbContext), IModuleTemplateRepository<TEntity> where TEntity : Entity;
