using CleanModularTemplate.Accounts.Domain.Customers.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanModularTemplate.Accounts.Infrastructure.Persistence;

internal sealed class AccountDbContext : DbContext
{
  public const string DefaultSchema = "accounts";
  public AccountDbContext(DbContextOptions<AccountDbContext> options)
	  : base(options)
  {
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
	base.OnModelCreating(modelBuilder);
	modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountDbContext).Assembly);
	modelBuilder.HasDefaultSchema(DefaultSchema);
  }

  internal DbSet<Customer> Customers { get; set; }
}
