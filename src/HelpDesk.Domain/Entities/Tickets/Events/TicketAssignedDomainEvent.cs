namespace HelpDesk.Domain.Entities.Tickets.Events;

public sealed record TicketAssignedDomainEvent(int TicketId, int UserId, int? ActionByUserId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
