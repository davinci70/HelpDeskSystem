namespace HelpDesk.Application.Common.Models;

public sealed record TicketFilter(
    TicketStatus? Status,
    TicketPriority? Priority,
    int? CategoryId,
    string? Search,
    TicketSortField SortBy,
    bool Descending,
    int Page,
    int PageSize);