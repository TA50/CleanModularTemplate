namespace CleanModularTemplate.Shared.Messaging;

public interface ICommandHandler<in TCommand, TResult>
	where TCommand : ICommand<TResult>
{
  Task<TResult> Handle(TCommand command, CancellationToken ct = default);
}
