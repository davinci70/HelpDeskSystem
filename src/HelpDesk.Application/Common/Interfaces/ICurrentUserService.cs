namespace HelpDesk.Application.Common.Interfaces;

public interface ICurrentUserService
{
    public int? UserId { get; }

    string? Email { get; }

    string? FullName { get; }

    string? Role { get; }

    bool IsAuthenticated { get; }
}
