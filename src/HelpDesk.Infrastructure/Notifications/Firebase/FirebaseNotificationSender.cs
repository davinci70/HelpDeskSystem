using HelpDesk.Application.Common.Exceptions;

namespace HelpDesk.Infrastructure.Notifications.Firebase;

public sealed class FirebaseNotificationSender : INotificationSender
{
    public async Task SendAsync(string token, string title, string body, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(body);

        var message = new Message
        {
            Token = token,
            Notification = new Notification
            {
                Title = title,
                Body = body
            }
        };

        try
        {
            await FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
        }
        catch (FirebaseMessagingException ex)
            when (ex.MessagingErrorCode == MessagingErrorCode.Unregistered)
        {
            throw new InvalidNotificationDeviceException();
        }
    }
}
