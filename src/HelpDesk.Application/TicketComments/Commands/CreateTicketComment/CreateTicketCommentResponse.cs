namespace HelpDesk.Application.TicketComments.Commands.CreateTicketComment;

public sealed record CreateTicketCommentResponse(
        int Id,
        int TicketId,
        int CreatedByUserId,
        string Content,
        bool IsInternal,
        DateTime CreatedAtUtc,
        DateTime? UpdatedAtUtc
);
