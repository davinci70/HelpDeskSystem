namespace HelpDesk.Domain.Entities.Tickets.Events.TicketCommentEvents;

public sealed record TicketCommentUpdatedDomainEvent(int TicketId, int CommentId, int ActionByUserId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
