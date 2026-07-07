using HelpDesk.Application.Common.Extensions;

namespace HelpDesk.Application.Tickets.Queries.GetTickets;

public sealed class GetTicketsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetTicketsQuery, Result<PagedResult<GetTicketSummaryResponse>>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<PagedResult<GetTicketSummaryResponse>>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Tickets
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
