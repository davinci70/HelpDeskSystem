namespace HelpDesk.Application.TicketComments.Commands.UpdateTicketComment;

public sealed record UpdateTicketCommentCommand(
    int TicketId,
    int CommentId,
    string Content
) : ICommand<UpdateTicketCommentResponse>;
