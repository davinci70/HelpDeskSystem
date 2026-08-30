namespace HelpDesk.Domain.Entities.Users;

public sealed class User : AggregateRoot
{
    private readonly List<UserNotificationDevice> _notificationDevices = [];

    private User()
    {
    }

    private User(
        string fullName,
        string email,
        string passwordHash,
        UserRole role)
    {
        FullName = NormalizeFullName(fullName);
        Email = NormalizeEmail(email);
        PasswordHash = ValidatePasswordHash(passwordHash);

        if (!Enum.IsDefined(role))
            throw new ArgumentException(
                "Invalid user role.",
                nameof(role));

        Role = role;
    }

    public IReadOnlyCollection<UserNotificationDevice> NotificationDevices => _notificationDevices.AsReadOnly();

    public string FullName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public UserRole Role { get; private set; }

    public static User Create(
        string fullName,
        string email,
        string passwordHash,
        UserRole role = UserRole.Customer) =>
        new(
            fullName,
            email,
            passwordHash,
            role);

    public void ChangeFullName(string fullName)
    {
        FullName = NormalizeFullName(fullName);
    }

    public void ChangeEmail(string email)
    {
        Email = NormalizeEmail(email);
    }

    public void ChangePasswordHash(string passwordHash)
    {
        PasswordHash = ValidatePasswordHash(passwordHash);
    }

    public void ChangeRole(UserRole role)
    {
        if (!Enum.IsDefined(role))
            throw new ArgumentException(
                "Invalid user role.",
                nameof(role));

        Role = role;
    }

    public bool IsAgent()
    {
        return Role == UserRole.Agent;
    }

    private static string NormalizeFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException(
                "Full name cannot be empty.",
                nameof(fullName));
        }

        return fullName.Trim();
    }

    private static string NormalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(
                "Email cannot be empty.",
                nameof(email));
        }

        return email.Trim().ToLowerInvariant();
    }

    private static string ValidatePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException(
                "Password hash cannot be empty.",
                nameof(passwordHash));
        }

        return passwordHash;
    }
}
