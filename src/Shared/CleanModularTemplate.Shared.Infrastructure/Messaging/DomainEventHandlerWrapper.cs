using CleanModularTemplate.Shared.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CleanModularTemplate.Shared.Infrastructure.Messaging;

#pragma warning disable S2094
internal abstract class DomainEventHandlerBase
#pragma warning restore S2094
{
}

internal abstract class DomainEventHandlerWrapper : DomainEventHandlerBase
{
  public abstract Task Handle(IDomainEvent domainEvent, IServiceProvider serviceProvider,
	  CancellationToken cancellationToken);
}


#pragma warning disable CA1812
internal sealed class DomainEventHandlerWrapperImpl<TEvent> : DomainEventHandlerWrapper
	where TEvent : IDomainEvent
{
  public override async Task Handle(IDomainEvent domainEvent, IServiceProvider serviceProvider,
	  CancellationToken cancellationToken)
  {
	var handlers = serviceProvider.GetServices<IDomainEventHandler<TEvent>>();
	foreach (var handler in handlers)
	{
	  await handler.Handle((TEvent)domainEvent, cancellationToken).ConfigureAwait(false);
	}
  }
}

#pragma warning restore CA1040
