using CleanModularTemplate.Shared;
using CleanModularTemplate.Shared.Domain;

namespace CleanModularTemplate.ModuleTemplate.UseCases;

public interface IModuleTemplateRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{

}
