namespace HelpDesk.Domain.Entities.Users;

public class User : Entity
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Customer;

    public ICollection<Ticket> Tickets { get; set; } = [];
}
