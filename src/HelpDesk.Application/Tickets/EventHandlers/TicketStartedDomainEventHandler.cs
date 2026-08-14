namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketStartedDomainEventHandler
        : INotificationHandler<DomainEventNotification<TicketStartedDomainEvent>>
{
    private readonly IApplicationDbContext _context;

    public TicketStartedDomainEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task Handle(
        DomainEventNotification<TicketStartedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Started(notification.DomainEvent.TicketId, notification.DomainEvent.ActionByUserId));

        return Task.CompletedTask;
    }
}