using Ardalis.Specification.EntityFrameworkCore;
using CleanModularTemplate.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace CleanModularTemplate.Shared.Infrastructure.Persistence;

public class GenericEfRepository<TEntity, TDbContext>(TDbContext dbContext)
	: RepositoryBase<TEntity>(dbContext) where TEntity : Entity
	where TDbContext : DbContext
{
}
