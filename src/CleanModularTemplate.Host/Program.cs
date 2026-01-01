using System.Diagnostics;
using System.Reflection;
using CleanModularTemplate.Accounts.Api;
using CleanModularTemplate.Host;
using CleanModularTemplate.IAM.Api;
using CleanModularTemplate.ModuleTemplate.Api;
using CleanModularTemplate.ModuleTemplate.Infrastructure;
using CleanModularTemplate.ServiceDefaults;
using CleanModularTemplate.Shared.Infrastructure;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using ProblemDetails = FastEndpoints.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

List<Assembly> assemblies = [];

builder.AddIam(assemblies)
	.AddAccounts(assemblies)
	.AddModuleTemplate(assemblies);

builder.AddSharedServices()
	.AddMessaging(assemblies);

builder.Services.AddFastEndpoints()
	.ConfigureSwaggerDocumentApi();



builder.Services.AddOpenTelemetry()
	.WithTracing(tracing => tracing
		.AddEntityFrameworkCoreInstrumentation()
	)
	.WithMetrics(metrics =>
	{
	  metrics
		  .AddNpgsqlInstrumentation();
	});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
  app.MapScalarApiReference(options =>
  {
	// Point Scalar to the FastEndpoints generated JSON
	options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");

	// Optional: Customize Title/Theme
	options.WithTitle("CleanModularTemplate API Docs");
	options.WithTheme(ScalarTheme.BluePlanet);
  });
}
app.UseExceptionHandler(errApp =>
{
  errApp.Run(async ctx =>
  {
	ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
	ctx.Response.ContentType = "application/problem+json";
	Console.WriteLine("TraceID" + Activity.Current?.TraceId);
	var traceId = Activity.Current?.TraceId.ToString();
	var response = new ProblemDetails
	{
	  TraceId = traceId ?? "",
	  Status = StatusCodes.Status500InternalServerError,
	};
	await ctx.Response.WriteAsJsonAsync(response);
  });
});
app.UseAuthentication()
	.UseAuthorization();

app.UseFastEndpoints(cfg =>
{
  cfg.Security.PermissionsClaimType = "permission";
  cfg.Errors.UseProblemDetails(x =>
  {
	x.AddRfcTitles();
	x.AddRfcTypes();
	x.ResponseBuilder = (failures, ctx, statusCode) =>
	  {
		var traceId = Activity.Current?.TraceId.ToString();
		return new ValidationProblemDetails(
		  failures.GroupBy(f => f.PropertyName)
			  .ToDictionary(
			  keySelector: e => e.Key,
			  elementSelector: e => e.Select(m => m.ErrorCode).ToArray())
		  )
		{
		  Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
		  Title = "One or more validation errors occurred.",
		  Status = statusCode,
		  Instance = ctx.Request.Path,
		  Extensions =
			  {
					{
						"traceId", traceId
					}
			  }
		};
	  };
  });
  cfg.Endpoints.Configurator = ep =>
  {
	ep.Description(b => b
		  .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
		  .ProducesProblemFE<ValidationProblemDetails>(contentType: "application/problem+json")
	  );

	Console.WriteLine("Configuring endpoint");
	if (ep.AllowedPermissions?.Count > 0)
	{
	  var permissionStr = string.Join(", ", ep.AllowedPermissions.Select(x => $"`{x}`"));
	  ep.Summary(s => { s.Description += $"\n\n**Required Permissions:** {permissionStr}"; });
	}
  };
}).UseSwaggerGen();
app.MapDefaultEndpoints();
await app.RunAsync();
