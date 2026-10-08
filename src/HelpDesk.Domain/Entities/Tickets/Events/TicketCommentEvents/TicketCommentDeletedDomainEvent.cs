namespace HelpDesk.Domain.Entities.Tickets.Events.TicketCommentEvents;

public sealed record TicketCommentDeletedDomainEvent(int TicketId, int CommentId, int ActionByUserId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
