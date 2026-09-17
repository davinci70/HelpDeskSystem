namespace HelpDesk.API.Contracts.TicketComments;

public record CreateTicketCommentRequest(
    string Content,
    bool IsInternal
);
