namespace HelpDesk.Application.TicketComments.Commands.CreateTicketComment;

public sealed record CreateTicketCommentCommand(
    int TicketId,
    string Content,
    bool IsInternal
) : ICommand<CreateTicketCommentResponse>;
