namespace CleanModularTemplate.Shared.Infrastructure;

public class SystemTimeProvider : ITimeProvider
{
  public DateTimeOffset GetUtcNow() => DateTimeOffset.UtcNow;
}
