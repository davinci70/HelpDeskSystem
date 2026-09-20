namespace HelpDesk.Application.Errors;

public static class TicketCommentErrors
{
    public static readonly Error InternalCommentNotAllowed =
        new ("TicketComments.InternalCommentNotAllowed", "Internal comments are not allowed for this user.", StatusCodes.Status403Forbidden);

    public static readonly Error CommentNotFound =
        new ("TicketComments.NotFound", "The ticket comment was not found", StatusCodes.Status404NotFound);
    
    public static readonly Error CommentAccessDenied =
        new ("TicketComments.AccessDenied", "Access to the ticket comment is denied", StatusCodes.Status403Forbidden);
}
