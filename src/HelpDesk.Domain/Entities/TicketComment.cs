using HelpDesk.Domain.Common.Primitives;
using HelpDesk.Domain.Entities.Tickets;

namespace HelpDesk.Domain.Entities;

public sealed class TicketComment : Entity
{
    public int TicketId { get; private set; }
    public string Content { get; private set; } = null!;
    public DateTime CreatedAtUtc { get; private set; }

    public Ticket Ticket { get; private set; } = null!;

    public TicketComment()
    {
        
    }

    public TicketComment(int ticketId, string content)
    {
        if (ticketId <= 0)
        {
            throw new ArgumentException("Ticket ID must be greater than 0.", nameof(ticketId));
        }

        SetContent(content);

        Content = content;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void UpdateContent(string content)
    {
        SetContent(content);
    }

    private void SetContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Content cannot be empty.", nameof(content));
        }
        
        Content = content.Trim();
    }
}
