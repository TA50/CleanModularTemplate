using System.Collections.Concurrent;
using CleanModularTemplate.Shared.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace CleanModularTemplate.Shared.Infrastructure.Messaging;

public sealed class InMemoryBus : ISender, IPublisher
{
  private readonly IServiceProvider _serviceProvider;

  public InMemoryBus(IServiceProvider serviceProvider)
  {
	_serviceProvider = serviceProvider;
  }

  private static readonly ConcurrentDictionary<Type, CommandHandlerBase> _commandWrappers = new();
  private static readonly ConcurrentDictionary<Type, EventHandlerWrapper> _eventWrappers = new();

  public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command,
	  CancellationToken cancellationToken = default)
  {
	ArgumentNullException.ThrowIfNull(command);

	var commandType = command.GetType();

	var wrapper = _commandWrappers.GetOrAdd(commandType, type =>
	{
	  var wrapperType = typeof(CommandHandlerWrapperImpl<,>).MakeGenericType(type, typeof(TResponse));
	  return (CommandHandlerBase)Activator.CreateInstance(wrapperType)!;
	});

	var result = await wrapper.Handle(command, _serviceProvider, cancellationToken).ConfigureAwait(false);

	return (TResponse)result!;
  }

  public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
	  where TEvent : IEvent
  {
	ArgumentNullException.ThrowIfNull(@event);

	var eventType = @event.GetType();

	// 1. Get or Create the Wrapper for this specific Event type
	var wrapper = _eventWrappers.GetOrAdd(eventType, type =>
	{
	  var wrapperType = typeof(EventHandlerWrapperImpl<>).MakeGenericType(type);
	  return (EventHandlerWrapper)Activator.CreateInstance(wrapperType)!;
	});

	await wrapper.Handle(@event, _serviceProvider, cancellationToken).ConfigureAwait(false);
  }


  public void Publish<TEvent>(TEvent ev) where TEvent : IEvent
  {
	Task.Run(async () => { await PublishAsync(ev); });
  }
}
