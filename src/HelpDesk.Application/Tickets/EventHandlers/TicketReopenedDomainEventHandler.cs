namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketReopenedDomainEventHandler(IApplicationDbContext context, INotificationService notificationService)
        : INotificationHandler<DomainEventNotification<TicketReopenedDomainEvent>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly INotificationService _notificationService = notificationService;

    public async Task Handle(
        DomainEventNotification<TicketReopenedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Reopened(notification.DomainEvent.TicketId, notification.DomainEvent.ActionByUserId));

        var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == notification.DomainEvent.TicketId, cancellationToken);

        if (ticket is null)
            return;

        await _notificationService.SendToUserAsync(
            ticket.CreatedByUserId,
            "Ticket Reopened",
            $"Ticket #{ticket.Id} has been reopened.",
            cancellationToken);
    }
}