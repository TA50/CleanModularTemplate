namespace CleanModularTemplate.Shared.Messaging;


public interface IPublisher
{
  /// <summary>
  /// This will wait for the handler to complete
  /// </summary>
  /// <param name="ev"></param>
  /// <param name="cancellationToken"></param>
  /// <typeparam name="TEvent"></typeparam>
  /// <returns></returns>
  Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;
  /// <summary>
  /// This will not wait for the handler to complete
  /// </summary>
  /// <param name="ev"></param>
  /// <typeparam name="TEvent"></typeparam>
  void Publish<TEvent>(TEvent ev) where TEvent : IEvent;
}
