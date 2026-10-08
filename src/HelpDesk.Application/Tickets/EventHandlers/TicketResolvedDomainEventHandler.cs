using HelpDesk.Domain.Entities.Tickets.Events.TicketEvents;

namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketResolvedDomainEventHandler(IApplicationDbContext context, INotificationService notificationService) 
        : INotificationHandler<DomainEventNotification<TicketResolvedDomainEvent>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly INotificationService _notificationService = notificationService;

    public async Task Handle(
        DomainEventNotification<TicketResolvedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Resolved(notification.DomainEvent.TicketId, notification.DomainEvent.ActionByUserId));

        var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == notification.DomainEvent.TicketId, cancellationToken);

        if (ticket is null)
            return;

        await _notificationService.SendToUserAsync(ticket.CreatedByUserId, "Ticket Resolved", $"Ticket #{ticket.Id} has been resolved.", cancellationToken);
    }
}
