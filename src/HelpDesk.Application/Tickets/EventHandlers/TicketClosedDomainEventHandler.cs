namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketClosedDomainEventHandler(IApplicationDbContext context)
        : INotificationHandler<DomainEventNotification<TicketClosedDomainEvent>>
{
    private readonly IApplicationDbContext _context = context;

    public Task Handle(
        DomainEventNotification<TicketClosedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Closed(notification.DomainEvent.TicketId, notification.DomainEvent.ActionByUserId));

        return Task.CompletedTask;
    }
}