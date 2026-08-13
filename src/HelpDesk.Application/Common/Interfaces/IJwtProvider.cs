namespace HelpDesk.Application.Common.Interfaces;

public interface IJwtProvider
{
    string Generate(User user);
}
