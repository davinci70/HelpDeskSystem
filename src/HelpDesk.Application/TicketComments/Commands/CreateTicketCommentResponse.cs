namespace HelpDesk.Application.TicketComments.Commands;

public sealed record CreateTicketCommentResponse(
        int Id,
        int TicketId,
        int CreatedByUserId,
        string Content,
        bool IsInternal,
        DateTime CreatedAtUtc,
        DateTime? UpdatedAtUtc
);
