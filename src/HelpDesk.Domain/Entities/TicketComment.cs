namespace HelpDesk.Domain.Entities;

public sealed class TicketComment : Entity
{
    private TicketComment()
    {
    }

    private TicketComment(string content)
    {
        Content = NormalizeContent(content);
        CreatedAtUtc = DateTime.UtcNow;
    }

    public int TicketId { get; private set; }

    public string Content { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    public Ticket Ticket { get; private set; } = null!;

    public static TicketComment Create(string content)
    {
        return new TicketComment(content);
    }

    public void UpdateContent(string content)
    {
        Content = NormalizeContent(content);
    }

    private static string NormalizeContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException(
                "Content cannot be empty.",
                nameof(content));
        }

        return content.Trim();
    }
}
