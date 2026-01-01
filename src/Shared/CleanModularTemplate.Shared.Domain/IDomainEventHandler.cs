namespace CleanModularTemplate.Shared.Domain;

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
  Task Handle(TEvent domainEvent, CancellationToken ct = default);
}
