namespace HelpDesk.Application.TicketComments.Commands.DeleteTicketComment;

public class DeleteTicketCommentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<DeleteTicketCommentCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result> Handle(DeleteTicketCommentCommand request, CancellationToken cancellationToken)
    {
        if (!await UserHasAccessToTicket(request.TicketId, cancellationToken))
            return Result.Failure(TicketErrors.TicketNotFound);

        var comment = await _context.TicketComments
            .Include(c => c.Ticket)
            .FirstOrDefaultAsync(c => c.Id == request.CommentId && c.TicketId == request.TicketId, cancellationToken);

        if (comment is null)
            return Result.Failure(TicketCommentErrors.CommentNotFound);

        if (!UserCanDeleteComment(comment))
            return Result.Failure(TicketCommentErrors.CommentAccessDenied);

        comment.Delete(_currentUser.UserId!.Value);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<bool> UserHasAccessToTicket(int ticketId, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);

        if (ticket is null)
            return false;

        return _currentUser.Role switch
        {
            nameof(UserRole.Admin)
                => true,

            nameof(UserRole.Customer)
                => ticket.CreatedByUserId == _currentUser.UserId,

            nameof(UserRole.Agent)
                => ticket.AssigneeId == _currentUser.UserId,

            _ => false
        };
    }

    private bool UserCanDeleteComment(TicketComment comment)
    {
        return _currentUser.Role switch
        {
            nameof(UserRole.Admin)
                => true,

            nameof(UserRole.Customer) or nameof(UserRole.Agent)
                => comment.CreatedByUserId == _currentUser.UserId,

            _ => false
        };
    }
}
