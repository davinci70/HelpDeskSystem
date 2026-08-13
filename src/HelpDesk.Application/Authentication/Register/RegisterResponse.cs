namespace HelpDesk.Application.Authentication.Register;

public record RegisterResponse(
    int Id,
    string FullName,
    string Email
);
