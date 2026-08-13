namespace HelpDesk.Domain.Entities.Tickets.Events;

public sealed record TicketStartedDomainEvent(int TicketId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
