using HelpDesk.Application.Common.Extensions;

namespace HelpDesk.Application.Tickets.Queries.GetTickets;

public sealed class GetTicketsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser) 
    : IRequestHandler<GetTicketsQuery, Result<PagedResult<GetTicketSummaryResponse>>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result<PagedResult<GetTicketSummaryResponse>>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Tickets.AsQueryable();

        if (_currentUser.Role == UserRole.Customer.ToString())
        {
            query = _context.Tickets.Where(t => t.CreatedByUserId == _currentUser.UserId);
        }
        else if (_currentUser.Role == UserRole.Agent.ToString())
        {
            query = _context.Tickets.Where(t => t.AssigneeId == _currentUser.UserId);
        }
        else
        {
            query = _context.Tickets;
        }

        query = query 
             .AsNoTracking()
             .FilterByStatus(request.Status)
             .FilterByPriority(request.Priority)
             .FilterByCategory(request.CategoryId)
             .Search(request.Search)
             .ApplySorting(request.SortBy, request.Descending);

        var result = await query
            .ProjectToType<GetTicketSummaryResponse>()
            .ToPagedResultAsync(
                request.Page,
                request.PageSize,
                cancellationToken);

        return Result.Success(result);
    }
}
