namespace HelpDesk.Application.Errors;

public class UserErrors
{
    public static Error EmailAlreadyExists =
        new("Users.EmailAlreadyExists", "Another user with this email already exists.", StatusCodes.Status409Conflict);
    
    public static Error InvalidCredentials =
        new("Users.InvalidCredentials", "Invalid email or password.", StatusCodes.Status404NotFound);
}
