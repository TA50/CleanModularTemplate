using Aspire.Hosting.Lifecycle;
using CleanModularTemplate.AspireHost;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);
builder.AddDockerComposeEnvironment("compose");

var postgres = builder.AddPostgres("postgres")
	.WithLifetime(ContainerLifetime.Persistent)
	.WithPgAdmin(); // Optional: UI for DB management

var defaultDb = postgres.AddDatabase("Default");
var iamSchema = defaultDb.AddSchema("IamDb", "iam");
var accountsSchema = defaultDb.AddSchema("AccountsDb", "accounts");

var databaseInitializer = builder
	.AddProject<CleanModularTemplate_DatabaseInitializer>("database-initializer")
	.WaitFor(defaultDb)
	.PublishAsDockerComposeService((resource, service) => { service.Name = "databaseInitializer"; });
AddDatabases(databaseInitializer);

var api = builder.AddProject<CleanModularTemplate_Host>("api")
	.WithReference(defaultDb)
	.PublishAsDockerComposeService((resource, service) => { service.Name = "api"; })
	.WaitForCompletion(databaseInitializer);
AddDatabases(api);

await builder.Build().RunAsync();



void AddDatabases<TDestination>(IResourceBuilder<TDestination> resourceBuilder)
	where TDestination : class, IResourceWithEnvironment
{
  resourceBuilder
	  .WithReference(iamSchema)
	  .WithReference(accountsSchema);
}
