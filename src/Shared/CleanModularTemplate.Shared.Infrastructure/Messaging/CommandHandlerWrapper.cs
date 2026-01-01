using CleanModularTemplate.Shared.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace CleanModularTemplate.Shared.Infrastructure.Messaging;

#pragma warning disable S1694
internal abstract class CommandHandlerBase
#pragma warning restore S1694
{
  public abstract Task<object?> Handle(object request, IServiceProvider serviceProvider,
	  CancellationToken cancellationToken);
}

internal abstract class CommandHandlerWrapper<TResponse> : CommandHandlerBase
{
  public abstract Task<TResponse> Handle(ICommand<TResponse> request, IServiceProvider serviceProvider,
	  CancellationToken cancellationToken);
}

#pragma warning disable CA1812
internal sealed class CommandHandlerWrapperImpl<TRequest, TResponse> : CommandHandlerWrapper<TResponse>
	where TRequest : ICommand<TResponse>
{
  public override async Task<object?> Handle(object request, IServiceProvider serviceProvider,
	  CancellationToken cancellationToken) =>
	  await Handle((ICommand<TResponse>)request, serviceProvider, cancellationToken).ConfigureAwait(false);

  public override Task<TResponse> Handle(ICommand<TResponse> request, IServiceProvider serviceProvider,
	  CancellationToken cancellationToken)
  {
	return serviceProvider.GetRequiredService<ICommandHandler<TRequest, TResponse>>()
		.Handle((TRequest)request, cancellationToken);
  }
}
#pragma warning restore CA1812
