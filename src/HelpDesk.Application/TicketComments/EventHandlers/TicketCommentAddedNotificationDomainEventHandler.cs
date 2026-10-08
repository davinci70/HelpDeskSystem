//using HelpDesk.Domain.Entities.Tickets.Events.TicketCommentEvents;

//namespace HelpDesk.Application.TicketComments.EventHandlers;

//public sealed class TicketCommentAddedNotificationDomainEventHandler(IApplicationDbContext context, INotificationService notificationService)
//    : INotificationHandler<DomainEventNotification<TicketCommentAddedDomainEvent>>
//{
//    private readonly IApplicationDbContext _context = context;
//    private readonly INotificationService _notificationService = notificationService;

//    public async Task Handle(DomainEventNotification<TicketCommentAddedDomainEvent> notification, CancellationToken cancellationToken)
//    {
//        var domainEvent = notification.DomainEvent;
//        var comment = domainEvent.Comment;

//        var ticket = await _context.Tickets
//            .AsNoTracking()
//            .FirstOrDefaultAsync(x => x.Id == comment.TicketId, cancellationToken);

//        if (ticket is null)
//            return;

//        var author = await _context.Users
//            .AsNoTracking()
//            .FirstOrDefaultAsync(x => x.Id == domainEvent.ActionByUserId, cancellationToken);

//        if (author is null)
//            return;

//        var recipientId = GetRecipientId(ticket, comment.IsInternal, domainEvent.ActionByUserId);

//        if (!recipientId.HasValue)
//            return;

//        var title = comment.IsInternal ? "New internal comment" : "New comment on your ticket";

//        var body = comment.IsInternal
//            ? $"{author.FullName} added an internal comment to ticket #{ticket.Id}."
//            : $"{author.FullName} added a new comment to ticket #{ticket.Id}.";

//        await _notificationService.SendToUserAsync(recipientId.Value, title, body, cancellationToken);
//    }

//    private int? GetRecipientId(Ticket ticket, bool isInternal, int actionByUserId)
//    {
//        if (isInternal)
//        {
//            if (!ticket.AssigneeId.HasValue)
//                return null;

//            return ticket.AssigneeId.Value == actionByUserId ? null : ticket.AssigneeId.Value;
//        }

//        return ticket.CreatedByUserId == actionByUserId ? ticket.AssigneeId : ticket.CreatedByUserId;
//    }
//}