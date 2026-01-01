namespace CleanModularTemplate.Shared.Messaging;

public interface ISender
{

  Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
}
