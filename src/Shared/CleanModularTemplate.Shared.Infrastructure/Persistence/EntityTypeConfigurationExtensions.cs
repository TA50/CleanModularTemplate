using CleanModularTemplate.Shared.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanModularTemplate.Shared.Infrastructure.Persistence;

public static class EntityTypeConfigurationExtensions
{
  public static void ConfigureAuditable<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, IAuditable
  {
	builder.Property(x => x.CreatedAt).IsRequired();
	builder.Property(x => x.CreatedBy).IsRequired();
	builder.Property(x => x.UpdatedAt).IsRequired();
	builder.Property(x => x.UpdatedBy).IsRequired();
  }

  public static void ConfigureDeletable<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, IDeletable
  {
	builder.Property(x => x.DeletedAt);
	builder.Property(x => x.DeletedBy);
  }
}
