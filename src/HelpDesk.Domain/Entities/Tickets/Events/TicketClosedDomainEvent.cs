namespace HelpDesk.Domain.Entities.Tickets.Events;

public sealed record TicketClosedDomainEvent(int TicketId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
