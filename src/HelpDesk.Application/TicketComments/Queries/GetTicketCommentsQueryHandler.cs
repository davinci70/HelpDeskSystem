namespace HelpDesk.Application.TicketComments.Queries;

public sealed class GetTicketCommentsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<GetTicketCommentsQuery, Result<PagedResult<GetTicketCommentsResponse>>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result<PagedResult<GetTicketCommentsResponse>>> Handle(GetTicketCommentsQuery request, CancellationToken cancellationToken)
    {
        if (!await UserHasAccessToTicket(request.TicketId, cancellationToken))
            return Result.Failure<PagedResult<GetTicketCommentsResponse>>(TicketErrors.TicketNotFound);

        var query = _context.TicketComments
            .AsNoTracking();

        var comments = await ApplyCommentVisibility(query)
            .Where(t => t.TicketId == request.TicketId && t.DeletedAtUtc == null)
            .OrderBy(tc => tc.CreatedAtUtc)
            .ThenBy(tc => tc.Id)
            .Select(tc => new GetTicketCommentsResponse(
                tc.Id,
                tc.TicketId,             
                tc.CreatedByUserId,
                tc.CreatedByUser.FullName,
                tc.Content,
                tc.IsInternal,
                tc.CreatedAtUtc,
                tc.UpdatedAtUtc
            ))
            .ToPagedResultAsync(request.Page, request.PageSize, cancellationToken);

        return Result.Success(comments);
    }


    private IQueryable<TicketComment> ApplyCommentVisibility(IQueryable<TicketComment> query)
    {
        if (_currentUser.Role == nameof(UserRole.Customer))
            query = query.Where(t => !t.IsInternal);
        
        return query;
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
                => true,

            _ => false
        };
    }
}
