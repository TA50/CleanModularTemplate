using System.Diagnostics.CodeAnalysis;
using CleanModularTemplate.Shared.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace CleanModularTemplate.Shared.Infrastructure.Messaging;

[SuppressMessage("Minor Code Smell", "S2094:Classes should not be empty")]
internal abstract class EventHandlerBase
{
}


internal abstract class EventHandlerWrapper : EventHandlerBase
{
  public abstract Task Handle(IEvent @event, IServiceProvider serviceProvider,
	  CancellationToken cancellationToken);
}



internal sealed class EventHandlerWrapperImpl<TEvent> : EventHandlerWrapper
	where TEvent : IEvent
{
  public override async Task Handle(IEvent @event, IServiceProvider serviceProvider,
	  CancellationToken cancellationToken)
  {
	var handlers = serviceProvider.GetServices<IEventHandler<TEvent>>();
	foreach (var handler in handlers)
	{
	  await handler.Handle((TEvent)@event, cancellationToken).ConfigureAwait(false);
	}
  }
}
