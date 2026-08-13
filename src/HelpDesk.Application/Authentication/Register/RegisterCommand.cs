namespace HelpDesk.Application.Authentication.Register;

public record RegisterCommand(
    string FullName,
    string Email,
    string Password
) : ICommand<RegisterResponse>; 
