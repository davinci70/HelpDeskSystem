namespace HelpDesk.Infrastructure.Messaging;

public sealed class DomainEventDispatcher(IPublisher publisher) : IDomainEventDispatcher
{
    private readonly IPublisher _publisher = publisher;

    public async Task DispatchAsync(
        IReadOnlyCollection<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var notificationType = typeof(DomainEventNotification<>)
                .MakeGenericType(domainEvent.GetType());

            var notification = (INotification)Activator.CreateInstance(
                notificationType,
                domainEvent)!;

            await _publisher.Publish(notification, cancellationToken);
        }
    }
}
