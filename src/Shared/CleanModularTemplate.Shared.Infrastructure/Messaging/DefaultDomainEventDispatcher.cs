using System.Collections.Concurrent;
using CleanModularTemplate.Shared.Domain;

namespace CleanModularTemplate.Shared.Infrastructure.Messaging;

public sealed class DefaultDomainEventDispatcher : IDomainEventDispatcher
{
  private readonly IServiceProvider _serviceProvider;

  private static readonly ConcurrentDictionary<Type, DomainEventHandlerWrapper> _eventWrappers = new();

  public DefaultDomainEventDispatcher(IServiceProvider serviceProvider)
  {
	_serviceProvider = serviceProvider;
  }


  public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default)
  {
	ArgumentNullException.ThrowIfNull(events);
	foreach (var domainEvent in events)
	{
	  await PublishAsync(domainEvent, ct);
	}
  }

  public void Dispatch(IEnumerable<IDomainEvent> events)
  {
  }

  private async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
	  where TEvent : IDomainEvent
  {
	ArgumentNullException.ThrowIfNull(domainEvent);

	var eventType = domainEvent.GetType();

	// 1. Get or Create the Wrapper for this specific Event type
	var wrapper = _eventWrappers.GetOrAdd(eventType, type =>
	{
	  var wrapperType = typeof(DomainEventHandlerWrapperImpl<>).MakeGenericType(type);
	  return (DomainEventHandlerWrapper)Activator.CreateInstance(wrapperType)!;
	});

	await wrapper.Handle(domainEvent, _serviceProvider, cancellationToken).ConfigureAwait(false);
  }
}
