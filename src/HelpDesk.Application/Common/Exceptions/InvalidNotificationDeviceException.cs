namespace HelpDesk.Application.Common.Exceptions;

public sealed class InvalidNotificationDeviceException : Exception
{
    public InvalidNotificationDeviceException() : base("The notification device is no longer registered with Firebase.")
    {
        
    }
}
