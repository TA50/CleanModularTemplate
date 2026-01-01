using CleanModularTemplate.Shared;
using CleanModularTemplate.Shared.Domain;

namespace CleanModularTemplate.Accounts.UseCases;

public interface IAccountRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{

}
