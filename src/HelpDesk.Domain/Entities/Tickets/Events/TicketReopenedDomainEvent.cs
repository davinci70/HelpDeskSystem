namespace HelpDesk.Domain.Entities.Tickets.Events;

public sealed record TicketReopenedDomainEvent(int TicketId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
