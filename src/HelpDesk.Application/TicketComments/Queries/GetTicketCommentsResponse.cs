namespace HelpDesk.Application.TicketComments.Queries;

public sealed record GetTicketCommentsResponse(
    int Id,
    int TicketId,
    int CreatedByUserId,
    string CreatedByUserFullName,
    string Content,
    bool IsInternal,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);
