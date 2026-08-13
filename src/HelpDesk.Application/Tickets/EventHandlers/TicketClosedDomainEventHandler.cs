namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketClosedDomainEventHandler
    : INotificationHandler<DomainEventNotification<TicketClosedDomainEvent>>
{
    private readonly IApplicationDbContext _context;

    public TicketClosedDomainEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task Handle(
        DomainEventNotification<TicketClosedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Closed(notification.DomainEvent.TicketId));

        return Task.CompletedTask;
    }
}