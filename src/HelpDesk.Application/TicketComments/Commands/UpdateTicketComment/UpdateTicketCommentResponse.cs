namespace HelpDesk.Application.TicketComments.Commands.UpdateTicketComment;

public sealed record UpdateTicketCommentResponse(
    int Id,
        int TicketId,
        int CreatedByUserId,
        string Content,
        bool IsInternal,
        DateTime CreatedAtUtc,
        DateTime? UpdatedAtUtc
);