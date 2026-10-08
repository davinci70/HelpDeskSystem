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

        if (!await UserHasAccessToTicket(ticket, cancellationToken))
            return Result.Failure<CreateTicketCommentResponse>(TicketErrors.TicketNotFound);

        if (request.IsInternal && !IsUserAdminOrAgent())
            return Result.Failure<CreateTicketCommentResponse>(TicketCommentErrors.InternalCommentNotAllowed);

        if (!request.IsInternal && IsTicketNotAssignedToCurrentAgent(ticket))
            return Result.Failure<CreateTicketCommentResponse>(TicketCommentErrors.PublicCommentNotAllowed);

        var comment = ticket.AddComment(request.Content, _currentUserService.UserId!.Value, request.IsInternal);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(comment.Adapt<CreateTicketCommentResponse>());
    }

    private async Task<bool> UserHasAccessToTicket(Ticket ticket, CancellationToken cancellationToken)
    {
        return _currentUserService.Role switch
        {
            nameof(UserRole.Admin)
                => true,

            nameof(UserRole.Customer)
                => ticket.CreatedByUserId == _currentUserService.UserId,

            nameof(UserRole.Agent)
                => true,

            _ => false
        };
    }

    private bool IsUserAdminOrAgent()
    {
        return _currentUserService.Role == UserRole.Admin.ToString() || _currentUserService.Role == UserRole.Agent.ToString();
    }

    private bool IsTicketNotAssignedToCurrentAgent(Ticket ticket)
    {
        return _currentUserService.Role == UserRole.Agent.ToString() && ticket.AssigneeId != _currentUserService.UserId!.Value;
    }   
}
