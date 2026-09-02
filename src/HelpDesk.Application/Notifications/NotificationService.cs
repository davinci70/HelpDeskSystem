using HelpDesk.Application.Common.Exceptions;

namespace HelpDesk.Application.Notifications;

public class NotificationService(IApplicationDbContext context, INotificationSender notificationSender) : INotificationService
{
    private readonly IApplicationDbContext _context = context;
    private readonly INotificationSender _notificationSender = notificationSender;

    public async Task SendToUserAsync(int userId, string title, string body, CancellationToken cancellationToken = default)
    {
        var devices = await _context.UserNotificationDevices
            .Where(x => x.UserId == userId && x.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var device in devices)
        {
            try
            {
                await _notificationSender.SendAsync(device.FcmToken, title, body, cancellationToken);
            }
            catch (InvalidNotificationDeviceException)
            {
                device.Deactivate();
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
