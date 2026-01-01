using System.Reflection;
using CleanModularTemplate.IAM.Api.Data;
using CleanModularTemplate.IAM.Api.Services;
using CleanModularTemplate.Shared.Api;
using CleanModularTemplate.Shared.Infrastructure;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanModularTemplate.IAM.Api;

public static class IamExtensions
{
  public static IHostApplicationBuilder AddIam(this IHostApplicationBuilder builder, List<Assembly> assemblies)
  {
	ArgumentNullException.ThrowIfNull(builder);
	builder.Services.AddTransient<SmsService>();
	builder.AddDbContext<IamDbContext>("IamDb");
	builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>
		{
		  options.User.RequireUniqueEmail = false;
		  options.User.AllowedUserNameCharacters = "0123456789+";
		  options.Password.RequireDigit = false;
		  options.Password.RequiredLength = 0;
		  options.Password.RequireLowercase = false;
		  options.Password.RequireNonAlphanumeric = false;
		  options.Password.RequireUppercase = false;
		})
		.AddRoles<ApplicationRole>()
		.AddEntityFrameworkStores<IamDbContext>()
		.AddDefaultTokenProviders();

	builder.Services.Configure<BearerTokenOptions>(IdentityConstants.BearerScheme,
	options => { options.BearerTokenExpiration = TimeSpan.FromDays(365); });
	builder.Services.AddAuthentication(IdentityConstants.BearerScheme);
	builder.Services.AddAuthorization();

	assemblies.Add(typeof(IamExtensions).Assembly);
	return builder;
  }


  public static readonly SwaggerTagGroups IamTagGroups = new("IAM", new[]
  {
		"Iam"
	});

  public static async Task InitIamDatabase(this IHost app)
  {
	ArgumentNullException.ThrowIfNull(app);
	using var scope = app.Services.CreateScope();
	var context = scope.ServiceProvider.GetRequiredService<IamDbContext>();
	await context.Database.ExecuteSqlRawAsync($"CREATE SCHEMA IF NOT EXISTS {IamDbContext.DefaultSchema}");

	await context.Database.MigrateAsync();


	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
	await IamDbInitializer.SeedAsync(roleManager);
  }
}
