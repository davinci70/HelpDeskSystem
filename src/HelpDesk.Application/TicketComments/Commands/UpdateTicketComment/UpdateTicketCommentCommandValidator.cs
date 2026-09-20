namespace HelpDesk.Application.TicketComments.Commands.UpdateTicketComment;

public class UpdateTicketCommentCommandValidator : AbstractValidator<UpdateTicketCommentCommand>
{
    public UpdateTicketCommentCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty()
            .GreaterThan(0);
        
        RuleFor(x => x.CommentId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Content)
            .Must(content => !string.IsNullOrWhiteSpace(content))
            .MaximumLength(2000);
    }
}
