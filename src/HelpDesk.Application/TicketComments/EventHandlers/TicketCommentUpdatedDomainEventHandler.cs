using HelpDesk.Domain.Entities.Tickets.Events.TicketCommentEvents;

namespace HelpDesk.Application.TicketComments.EventHandlers;

public sealed class TicketCommentUpdatedDomainEventHandler(IApplicationDbContext context)
    : INotificationHandler<DomainEventNotification<TicketCommentUpdatedDomainEvent>>
{
    private readonly IApplicationDbContext _context = context;

    public Task Handle(DomainEventNotification<TicketCommentUpdatedDomainEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _context.TicketHistories.Add(TicketHistory.CommentUpdated(domainEvent.TicketId, domainEvent.CommentId, domainEvent.ActionByUserId));

        return Task.CompletedTask;
    }
}