namespace CleanModularTemplate.Shared.Domain;

public abstract class EntityWithDomainEvents<TId> : Entity<TId>, IHasDomainEvents where TId : struct
{
  private readonly List<IDomainEvent> _domainEvents = [];
  public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
  public void AddDomainEvent(IDomainEvent domainEvent)
  {
	_domainEvents.Add(domainEvent);
  }
  public void RemoveDomainEvent(IDomainEvent domainEvent)
  {
	_domainEvents.Remove(domainEvent);
  }
  public void ClearDomainEvents()
  {
	_domainEvents.Clear();
  }
  public void ClearDomainEvents<TEvent>() where TEvent : IDomainEvent
  {
	_domainEvents.RemoveAll(domainEvent => domainEvent is TEvent);
  }
}
