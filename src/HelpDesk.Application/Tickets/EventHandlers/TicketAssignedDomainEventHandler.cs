using HelpDesk.Domain.Entities.Tickets.Events;
namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketAssignedDomainEventHandler
    : INotificationHandler<DomainEventNotification<TicketAssignedDomainEvent>>
{
    private readonly IApplicationDbContext _context;

    public TicketAssignedDomainEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task Handle(
        DomainEventNotification<TicketAssignedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Assigned(
                notification.DomainEvent.TicketId,
                notification.DomainEvent.UserId));

        return Task.CompletedTask;
    }
}