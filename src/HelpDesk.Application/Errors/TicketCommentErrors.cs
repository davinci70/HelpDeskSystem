namespace HelpDesk.Application.Errors;

public static class TicketCommentErrors
{
    public static readonly Error InternalCommentNotAllowed =
        new ("TicketCommentErrors.InternalCommentNotAllowed", "Internal comments are not allowed for this user.", StatusCodes.Status403Forbidden);
}
