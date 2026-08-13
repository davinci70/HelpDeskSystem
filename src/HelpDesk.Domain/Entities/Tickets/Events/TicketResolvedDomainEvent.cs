namespace HelpDesk.Domain.Entities.Tickets.Events;

public sealed record TicketResolvedDomainEvent(int TicketId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
