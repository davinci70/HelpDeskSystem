namespace HelpDesk.Application.NotificationDevices.Commands.RegisterNotificationDevice;

public sealed record RegisterNotificationDeviceCommand(string FcmToken) : ICommand;