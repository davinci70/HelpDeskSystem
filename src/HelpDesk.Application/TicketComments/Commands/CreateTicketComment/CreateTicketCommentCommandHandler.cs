namespace HelpDesk.Application.TicketComments.Commands.CreateTicketComment;

public sealed class CreateTicketCommentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<CreateTicketCommentCommand, Result<CreateTicketCommentResponse>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<Result<CreateTicketCommentResponse>> Handle(CreateTicketCommentCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets.FindAsync([request.TicketId], cancellationToken);

        if (ticket is null)
            return Result.Failure<CreateTicketCommentResponse>(TicketErrors.TicketNotFound);

        if (request.IsInternal && !IsUserAdminOrAgent())
            return Result.Failure<CreateTicketCommentResponse>(TicketCommentErrors.InternalCommentNotAllowed);

        var comment = ticket.AddComment(request.Content, _currentUserService.UserId!.Value, request.IsInternal);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(comment.Adapt<CreateTicketCommentResponse>());
    }

    private bool IsUserAdminOrAgent()
    {
        return _currentUserService.Role == UserRole.Admin.ToString() || _currentUserService.Role == UserRole.Agent.ToString();
    }
}
