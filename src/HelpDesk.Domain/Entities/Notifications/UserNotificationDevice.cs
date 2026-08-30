namespace HelpDesk.Domain.Entities.Notifications;

public sealed class UserNotificationDevice : Entity
{
    private UserNotificationDevice()
    {
    }

    private UserNotificationDevice(
        int userId,
        string fcmToken)
    {
        if (userId <= 0)
        {
            throw new ArgumentException(
                "User ID must be greater than zero.",
                nameof(userId));
        }

        FcmToken = NormalizeToken(fcmToken);

        UserId = userId;
        CreatedAtUtc = DateTime.UtcNow;
        LastSeenAtUtc = DateTime.UtcNow;
        IsActive = true;
    }

    public int UserId { get; private set; }

    public string FcmToken { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime LastSeenAtUtc { get; private set; }

    public bool IsActive { get; private set; }

    public User User { get; private set; } = null!;

    public static UserNotificationDevice Create(
        int userId,
        string fcmToken)
    {
        return new UserNotificationDevice(
            userId,
            fcmToken);
    }
    public void ChangeUser(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException(
                "User Id must be greater than zero.",
                nameof(userId));
        }

        UserId = userId;
        LastSeenAtUtc = DateTime.UtcNow;
        IsActive = true;
    }

    public void UpdateToken(string fcmToken)
    {
        FcmToken = NormalizeToken(fcmToken);
        LastSeenAtUtc = DateTime.UtcNow;
        IsActive = true;
    }

    public void MarkAsSeen()
    {
        LastSeenAtUtc = DateTime.UtcNow;
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private static string NormalizeToken(string fcmToken)
    {
        if (string.IsNullOrWhiteSpace(fcmToken))
        {
            throw new ArgumentException(
                "FCM token cannot be empty.",
                nameof(fcmToken));
        }

        return fcmToken.Trim();
    }
}
