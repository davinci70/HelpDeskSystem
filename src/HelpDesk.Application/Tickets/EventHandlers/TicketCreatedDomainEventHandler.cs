namespace HelpDesk.Application.Tickets.EventHandlers;

public sealed class TicketCreatedDomainEventHandler
    : INotificationHandler<DomainEventNotification<TicketCreatedDomainEvent>>
{
    private readonly IApplicationDbContext _context;

    public TicketCreatedDomainEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task Handle(
        DomainEventNotification<TicketCreatedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        _context.TicketHistories.Add(
            TicketHistory.Created(notification.DomainEvent.Ticket.Id));

        return Task.CompletedTask;
    }
}