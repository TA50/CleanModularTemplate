namespace CleanModularTemplate.Shared.Messaging;

#pragma warning disable S2326
#pragma warning disable CA1040
public interface ICommand<out TResult>;

#pragma warning restore CA1040
#pragma warning restore S2326

