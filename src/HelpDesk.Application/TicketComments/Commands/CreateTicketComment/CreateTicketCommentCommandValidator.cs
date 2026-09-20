namespace HelpDesk.Application.TicketComments.Commands.CreateTicketComment;

public class CreateTicketCommentCommandValidator : AbstractValidator<CreateTicketCommentCommand>
{
    public CreateTicketCommentCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(x => x.TicketId)
            .GreaterThan(0);
    }
}
