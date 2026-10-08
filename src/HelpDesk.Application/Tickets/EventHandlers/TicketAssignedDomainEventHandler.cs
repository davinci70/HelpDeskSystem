using HelpDesk.Domain.Entities.Tickets.Events.TicketEvents;

namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketAssignedDomainEventHandler(IApplicationDbContext context, INotificationService notificationService)
        : INotificationHandler<DomainEventNotification<TicketAssignedDomainEvent>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly INotificationService _notificationService = notificationService;

    public async Task Handle(DomainEventNotification<TicketAssignedDomainEvent> notification, CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Assigned(
                notification.DomainEvent.TicketId,
                notification.DomainEvent.UserId,
                notification.DomainEvent.ActionByUserId));

        await _notificationService.SendToUserAsync(
            notification.DomainEvent.UserId,
            "Ticket Assigned",
            $"Ticket #{notification.DomainEvent.TicketId} has been assigned to you.",
            cancellationToken);
    }
}