namespace HelpDesk.Domain.Entities.Tickets.Events.TicketCommentEvents;

public sealed record TicketCommentAddedDomainEvent(TicketComment Comment, int ActionByUserId) : IDomainEvent
{
    public int TicketId => Comment.TicketId;

    public int CommentId => Comment.Id;

    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
