namespace HelpDesk.Domain.Entities.Tickets.Events.TicketEvents;

public sealed record TicketResolvedDomainEvent(int TicketId, int? ActionByUserId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
