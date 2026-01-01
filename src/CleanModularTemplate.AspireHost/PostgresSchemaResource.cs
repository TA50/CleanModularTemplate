using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;

namespace CleanModularTemplate.AspireHost;

internal sealed class PostgresSchemaResource(string name, string schemaName, PostgresDatabaseResource databaseParentResource)
	: Resource(name), IResourceWithParent<PostgresDatabaseResource>, IResourceWithConnectionString
{
  public PostgresDatabaseResource Parent { get; } = databaseParentResource ?? throw new ArgumentNullException(nameof(databaseParentResource));

  public ReferenceExpression ConnectionStringExpression
  {
	get
	{
	  var connectionStringBuilder = new DbConnectionStringBuilder
	  {
		["Search Path"] = $"\"{SchemaName}\""
	  };

	  return ReferenceExpression.Create($"{Parent};{connectionStringBuilder.ToString()}");
	}
  }

  public string SchemaName
  {
	get
	{
	  ArgumentException.ThrowIfNullOrEmpty(schemaName);
	  return schemaName;
	}
  }

}

internal static class PostgresHostingExtensions
{

  public static IResourceBuilder<PostgresSchemaResource> AddSchema(this IResourceBuilder<PostgresDatabaseResource> builder,
	  [ResourceName] string name, string? schemaName = null)
  {
	ArgumentNullException.ThrowIfNull(builder);
	ArgumentException.ThrowIfNullOrEmpty(name);
	schemaName ??= name;

	var postgresSchema = new PostgresSchemaResource(name, schemaName, builder.Resource);

	string? connectionString = null;

	builder.ApplicationBuilder.Eventing.Subscribe<ConnectionStringAvailableEvent>(postgresSchema, async (@event, ct) =>
	{
	  connectionString = await postgresSchema.ConnectionStringExpression.GetValueAsync(ct).ConfigureAwait(false);

	  if (connectionString == null)
	  {
		throw new DistributedApplicationException(
			$"ConnectionStringAvailableEvent was published for the '{name}' resource but the connection string was null.");
	  }
	});

	var healthCheckKey = $"{name}_check";
	builder.ApplicationBuilder.Services.AddHealthChecks()
		.AddNpgSql(sp => connectionString ?? throw new InvalidOperationException("Connection string is unavailable"), name: healthCheckKey);

	return builder.ApplicationBuilder
		.AddResource(postgresSchema)
		.WithHealthCheck(healthCheckKey);
  }
}
