namespace HelpDesk.Domain.Entities.Tickets.Events;

public sealed record TicketAssignedDomainEvent(int TicketId, int UserId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
