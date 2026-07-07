namespace HelpDesk.Application.Tickets.Queries.GetTickets;

public record GetTicketsQuery(
     int Page = 1,
    int PageSize = 10,
    TicketStatus? Status = null,
    TicketPriority? Priority = null,
    int? CategoryId = null,
    string? Search = null,
    TicketSortField SortBy = TicketSortField.CreatedAt,
    bool Descending = false
) : IQuery<PagedResult<GetTicketSummaryResponse>>;
