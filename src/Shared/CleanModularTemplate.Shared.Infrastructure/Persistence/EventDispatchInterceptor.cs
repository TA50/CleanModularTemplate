using CleanModularTemplate.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanModularTemplate.Shared.Infrastructure.Persistence;

public class EventDispatchInterceptor<TDbContext> : SaveChangesInterceptor where TDbContext : DbContext
{
  private readonly IDomainEventDispatcher _domainEventDispatcher;

  public EventDispatchInterceptor(IDomainEventDispatcher domainEventDispatcher)
  {
	_domainEventDispatcher = domainEventDispatcher;
  }

  // Called after SaveChangesAsync has completed successfully
  public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
	  CancellationToken cancellationToken = new CancellationToken())
  {
	ArgumentNullException.ThrowIfNull(eventData);
	var context = eventData.Context;
	if (context is not TDbContext appDbContext)
	{
	  return await base.SavedChangesAsync(eventData, result, cancellationToken).ConfigureAwait(false);
	}

	// Retrieve all tracked entities that have domain events
	var entitiesWithEvents = appDbContext.ChangeTracker.Entries<IHasDomainEvents>()
		.Select(e => e.Entity)
		.Where(e => e.DomainEvents.Count > 0)
		.ToArray();

	// Dispatch and clear domain events
	foreach (var entity in entitiesWithEvents)
	{
	  await _domainEventDispatcher.DispatchAsync(entity.DomainEvents, cancellationToken);
	  entity.ClearDomainEvents();
	}

	return await base.SavedChangesAsync(eventData, result, cancellationToken);
  }
}
