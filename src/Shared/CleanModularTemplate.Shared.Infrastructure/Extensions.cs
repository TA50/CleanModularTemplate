using System.Diagnostics;
using System.Reflection;
using CleanModularTemplate.Shared.Domain;
using CleanModularTemplate.Shared.Infrastructure.Messaging;
using CleanModularTemplate.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace CleanModularTemplate.Shared.Infrastructure;

public static class InfrastructureExtensions
{
  public static IHostApplicationBuilder AddSharedServices(this IHostApplicationBuilder builder)
  {
	builder.Services.AddSingleton<ITimeProvider, SystemTimeProvider>();
	return builder;
  }

  public static IHostApplicationBuilder AddMessaging(this IHostApplicationBuilder builder, List<Assembly> assemblies)
  {
	ArgumentNullException.ThrowIfNull(builder);
	ArgumentNullException.ThrowIfNull(assemblies);

	builder.Services.AddTransient<IDomainEventDispatcher, DefaultDomainEventDispatcher>();
	builder.Services.AddTransient<IPublisher, InMemoryBus>();
	builder.Services.AddTransient<ISender, InMemoryBus>();

	foreach (var assembly in assemblies)
	{
	  builder.AddEventHandlers(assembly);
	  builder.AddCommandHandlers(assembly);
	  builder.AddDomainEventHandlers(assembly);
	}

	return builder;
  }

  private static void AddEventHandlers(this IHostApplicationBuilder builder, Assembly assembly)
  {
	var types = assembly.GetTypes()
		.Where(t => t.IsClass && !t.IsAbstract)
		.SelectMany(type => type.GetInterfaces(), (type, interfaceType) => new
		{
		  Implementation = type,
		  Interface = interfaceType
		})
		.Where(t => t.Interface.IsGenericType &&
					t.Interface.GetGenericTypeDefinition() == typeof(IEventHandler<>));

	foreach (var t in types)
	{
	  builder.Services.AddTransient(t.Interface, t.Implementation);
	}
  }

  private static void AddCommandHandlers(this IHostApplicationBuilder builder, Assembly assembly)
  {
	var types = assembly.GetTypes()
		.Where(t => t.IsClass && !t.IsAbstract)
		.SelectMany(type => type.GetInterfaces(), (type, interfaceType) => new
		{
		  Implementation = type,
		  Interface = interfaceType
		})
		.Where(t => t.Interface.IsGenericType &&
					t.Interface.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

	foreach (var t in types)
	{
	  builder.Services.AddTransient(t.Interface, t.Implementation);
	}
  }

  private static void AddDomainEventHandlers(this IHostApplicationBuilder builder, Assembly assembly)
  {
	var types = assembly.GetTypes()
		.Where(t => t.IsClass && !t.IsAbstract)
		.SelectMany(type => type.GetInterfaces(), (type, interfaceType) => new
		{
		  Implementation = type,
		  Interface = interfaceType
		})
		.Where(t => t.Interface.IsGenericType &&
					t.Interface.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>));

	foreach (var t in types)
	{
	  builder.Services.AddTransient(t.Interface, t.Implementation);
	}
  }


  public static void AddDefaultDbContext<TDbContext>(this IHostApplicationBuilder builder) where TDbContext : DbContext
  {
	builder.AddDbContext<TDbContext>("Default");
  }
  public static void AddDbContext<TDbContext>(this IHostApplicationBuilder builder, string connectionStringName) where TDbContext : DbContext
  {
	builder.AddNpgsqlDbContext<TDbContext>(connectionStringName);
  }

}
