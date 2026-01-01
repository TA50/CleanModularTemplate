using CleanModularTemplate.Accounts.UseCases;
using CleanModularTemplate.Shared.Domain;
using CleanModularTemplate.Shared.Infrastructure.Persistence;

namespace CleanModularTemplate.Accounts.Infrastructure;

internal sealed class AccountEfRepository<TEntity>(AccountDbContext dbContext)
	: GenericEfRepository<TEntity, AccountDbContext>(dbContext), IAccountRepository<TEntity> where TEntity : Entity;
