using HelpDesk.Domain.Entities.Tickets.Events.TicketCommentEvents;

namespace HelpDesk.Application.TicketComments.EventHandlers;

public sealed class TicketCommentDeletedDomainEventHandler(IApplicationDbContext context)
    : INotificationHandler<DomainEventNotification<TicketCommentDeletedDomainEvent>>
{
    private readonly IApplicationDbContext _context = context;

    public Task Handle(
        DomainEventNotification<TicketCommentDeletedDomainEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _context.TicketHistories.Add(TicketHistory.CommentDeleted(domainEvent.TicketId, domainEvent.CommentId, domainEvent.ActionByUserId));

        return Task.CompletedTask;
    }
}