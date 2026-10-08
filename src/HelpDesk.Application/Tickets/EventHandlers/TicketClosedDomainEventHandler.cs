using HelpDesk.Domain.Entities.Tickets.Events.TicketEvents;

namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketClosedDomainEventHandler(IApplicationDbContext context, INotificationService notificationService)
        : INotificationHandler<DomainEventNotification<TicketClosedDomainEvent>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly INotificationService _notificationService = notificationService;

    public async Task Handle(
        DomainEventNotification<TicketClosedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Closed(notification.DomainEvent.TicketId, notification.DomainEvent.ActionByUserId));

        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == notification.DomainEvent.TicketId, cancellationToken);

        if (ticket is null)
            return;

        await _notificationService.SendToUserAsync(ticket.CreatedByUserId, "Ticket Closed", $"Ticket #{ticket.Id} has been closed.", cancellationToken);
    }
}