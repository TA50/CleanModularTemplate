using CleanModularTemplate.Accounts.Infrastructure.Persistence;
using CleanModularTemplate.Accounts.UseCases;
using CleanModularTemplate.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanModularTemplate.Accounts.Infrastructure;

public static class AccountInfrastructureExtensions
{
  public static void AddAccountInfrastructure(this IHostApplicationBuilder builder)
  {
	builder.AddDbContext<AccountDbContext>("AccountsDb");
	builder.Services.AddScoped(typeof(IAccountRepository<>), typeof(AccountEfRepository<>));
  }

  public static async Task InitAccountsDb(this IHost app)
  {
	ArgumentNullException.ThrowIfNull(app);
	using var scope = app.Services.CreateScope();
	var context = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
	await context.Database.ExecuteSqlRawAsync($"CREATE SCHEMA IF NOT EXISTS {AccountDbContext.DefaultSchema}");
	await context.Database.MigrateAsync();
  }
}
