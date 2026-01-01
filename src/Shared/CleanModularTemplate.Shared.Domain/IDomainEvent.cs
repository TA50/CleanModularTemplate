namespace CleanModularTemplate.Shared.Domain;

public interface IDomainEvent
{
  public DateTimeOffset RaisedAt { get; set; }

}
