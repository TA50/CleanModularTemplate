namespace CleanModularTemplate.Shared.Domain;

public interface IDeletable
{
  public DateTimeOffset? DeletedAt { get; set; }
  public Guid? DeletedBy { get; }

}
