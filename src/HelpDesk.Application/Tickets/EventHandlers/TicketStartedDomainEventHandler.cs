using HelpDesk.Domain.Entities.Tickets.Events.TicketEvents;

namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketStartedDomainEventHandler(IApplicationDbContext context, INotificationService notificationService)
                : INotificationHandler<DomainEventNotification<TicketStartedDomainEvent>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly INotificationService _notificationService = notificationService;

    public async Task Handle(
        DomainEventNotification<TicketStartedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Started(notification.DomainEvent.TicketId, notification.DomainEvent.ActionByUserId));

        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(x => x.Id == notification.DomainEvent.TicketId, cancellationToken);

        if (ticket is null)
            return;

        await _notificationService.SendToUserAsync(ticket.CreatedByUserId, "Ticket Started", $"Ticket #{ticket.Id} has been started.", cancellationToken);
    }
}