namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketReopenedDomainEventHandler
    : INotificationHandler<DomainEventNotification<TicketReopenedDomainEvent>>
{
    private readonly IApplicationDbContext _context;

    public TicketReopenedDomainEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task Handle(
        DomainEventNotification<TicketReopenedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Reopened(notification.DomainEvent.TicketId));

        return Task.CompletedTask;
    }
}