using HelpDesk.Domain.Entities.Tickets.Events.TicketCommentEvents;

namespace HelpDesk.Application.TicketComments.EventHandlers;

public sealed class TicketCommentAddedDomainEventHandler(IApplicationDbContext context) : 
    INotificationHandler<DomainEventNotification<TicketCommentAddedDomainEvent>>
{
    private readonly IApplicationDbContext _context = context;

    public Task Handle(DomainEventNotification<TicketCommentAddedDomainEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _context.TicketHistories.Add(TicketHistory.CommentAdded(domainEvent.TicketId, domainEvent.CommentId, domainEvent.ActionByUserId));

        return Task.CompletedTask;
    }
}