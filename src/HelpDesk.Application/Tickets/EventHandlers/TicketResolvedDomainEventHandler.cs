namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketResolvedDomainEventHandler(IApplicationDbContext context) 
        : INotificationHandler<DomainEventNotification<TicketResolvedDomainEvent>>
{
    private readonly IApplicationDbContext _context = context;

    public Task Handle(
        DomainEventNotification<TicketResolvedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Resolved(notification.DomainEvent.TicketId, notification.DomainEvent.ActionByUserId));

        return Task.CompletedTask;

    }
}
