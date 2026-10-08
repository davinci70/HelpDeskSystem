namespace HelpDesk.Application.TicketComments.Commands.UpdateTicketComment;

public sealed class UpdateTicketCommentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<UpdateTicketCommentCommand, Result<UpdateTicketCommentResponse>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result<UpdateTicketCommentResponse>> Handle(UpdateTicketCommentCommand request, CancellationToken cancellationToken)
    {
        if (!await UserHasAccessToTicket(request.TicketId, cancellationToken))
            return Result.Failure<UpdateTicketCommentResponse>(TicketErrors.TicketNotFound);

        var comment = await _context.TicketComments
            .Include(c => c.Ticket)
            .FirstOrDefaultAsync(c => c.Id == request.CommentId && c.TicketId == request.TicketId, cancellationToken);

        if (comment is null)
            return Result.Failure<UpdateTicketCommentResponse>(TicketCommentErrors.CommentNotFound);

        if (!UserCanEditComment(comment))
            return Result.Failure<UpdateTicketCommentResponse>(TicketCommentErrors.CommentAccessDenied);

        comment.Ticket.UpdateComment(comment, request.Content, _currentUser.UserId!.Value);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(new UpdateTicketCommentResponse(
                comment.Id,
                comment.TicketId,
                comment.CreatedByUserId,
                comment.Content,
                comment.IsInternal,
                comment.CreatedAtUtc,
                comment.UpdatedAtUtc)
            );
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

    private bool UserCanEditComment(TicketComment comment)
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
