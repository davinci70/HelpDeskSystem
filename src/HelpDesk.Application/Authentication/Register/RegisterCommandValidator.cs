namespace HelpDesk.Application.Authentication.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Email)
          .NotEmpty()
          .EmailAddress()
          .MaximumLength(150);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}
