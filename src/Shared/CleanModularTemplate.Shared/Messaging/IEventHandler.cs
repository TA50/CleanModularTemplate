namespace CleanModularTemplate.Shared.Messaging;

public interface IEventHandler<in TEvent> where TEvent : IEvent
{
  Task Handle(TEvent ev, CancellationToken ct = default);
}
