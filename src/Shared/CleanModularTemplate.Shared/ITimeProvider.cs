namespace CleanModularTemplate.Shared;

public interface ITimeProvider
{
  DateTimeOffset GetUtcNow();
}
