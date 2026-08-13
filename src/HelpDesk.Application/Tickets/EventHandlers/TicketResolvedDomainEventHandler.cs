namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketResolvedDomainEventHandler
    : INotificationHandler<
        DomainEventNotification<TicketResolvedDomainEvent>>
{
    private readonly IApplicationDbContext _context;

    public TicketResolvedDomainEventHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public Task Handle(
        DomainEventNotification<TicketResolvedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Resolved(notification.DomainEvent.TicketId));

        return Task.CompletedTask;

    }
}
