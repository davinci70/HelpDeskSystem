namespace HelpDesk.Application.TicketComments.Commands.DeleteTicketComment;

public record DeleteTicketCommentCommand(
    int TicketId,
    int CommentId
) : ICommand;
