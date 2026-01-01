namespace CleanModularTemplate.Shared.Domain;

public interface IAuditable
{
  public DateTimeOffset CreatedAt { get; }
  public Guid CreatedBy { get; }
  public DateTimeOffset UpdatedAt { get; }
  public Guid UpdatedBy { get; }

}
