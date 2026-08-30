namespace HelpDesk.Application.NotificationDevices.Commands.RegisterNotificationDevice;

public sealed class RegisterNotificationDeviceValidator : AbstractValidator<UserNotificationDevice>
{
    public RegisterNotificationDeviceValidator()
    {
        RuleFor(x => x.FcmToken)
            .NotEmpty()
            .MaximumLength(500);
    }
}
