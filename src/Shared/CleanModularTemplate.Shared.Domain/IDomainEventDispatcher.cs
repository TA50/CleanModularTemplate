namespace CleanModularTemplate.Shared.Domain;

public interface IDomainEventDispatcher
{
  Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default);
  void Dispatch(IEnumerable<IDomainEvent> events);
}
