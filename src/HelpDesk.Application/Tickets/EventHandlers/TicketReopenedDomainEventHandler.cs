namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketReopenedDomainEventHandler(IApplicationDbContext context)
        : INotificationHandler<DomainEventNotification<TicketReopenedDomainEvent>>
{
    private readonly IApplicationDbContext _context = context;

    public Task Handle(
        DomainEventNotification<TicketReopenedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Reopened(notification.DomainEvent.TicketId, notification.DomainEvent.ActionByUserId));

        return Task.CompletedTask;
    }
}