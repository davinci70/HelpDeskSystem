namespace HelpDesk.Domain.Entities.Tickets.Events.TicketEvents;

public sealed record TicketCreatedDomainEvent(Ticket Ticket) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
