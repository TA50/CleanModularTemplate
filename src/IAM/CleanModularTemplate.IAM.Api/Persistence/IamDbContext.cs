using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanModularTemplate.IAM.Api.Data;

// Use Guid as the TKey parameter

internal sealed class IamDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
  public const string DefaultSchema = "iam";

  public IamDbContext(DbContextOptions<IamDbContext> options) :
	  base(options)
  {
  }
  protected override void OnModelCreating(ModelBuilder builder)
  {
	builder.HasDefaultSchema(DefaultSchema);
	base.OnModelCreating(builder);
  }
}
