namespace HelpDesk.Application.Common.Interfaces;

public interface INotificationSender
{
    Task SendAsync(string token, string title, string body);
}
