namespace HelpDesk.Domain.Entities;

public class Agent : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = [];
}
