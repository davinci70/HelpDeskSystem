namespace HelpDesk.Application.TicketComments.Queries;

public sealed record GetTicketCommentsQuery(
    int TicketId,
    int Page = 1,
    int PageSize = 10
) : IQuery<PagedResult<GetTicketCommentsResponse>>;