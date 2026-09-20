namespace HelpDesk.Domain.Entities.Tickets;

public sealed class TicketComment : Entity
{
    private TicketComment()
    {
    }

    private TicketComment(string content, int createdByUserId, bool isInternal)
    {
        if (createdByUserId <= 0)
            throw new ArgumentException(
                "User ID must be greater than zero.",
                nameof(createdByUserId));

        Content = NormalizeContent(content);
        CreatedByUserId = createdByUserId;
        IsInternal = isInternal;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public int TicketId { get; private set; }
    public int CreatedByUserId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public bool IsInternal { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; } = null;
    public DateTime? DeletedAtUtc { get; private set; }
    public int? DeletedByUserId { get; private set; }

    public Ticket Ticket { get; private set; } = null!;
    public User CreatedByUser { get; private set; } = null!;
    public User? DeletedByUser { get; private set; }

    public static TicketComment Create(string content, int createdByUserId, bool isInternal)
    {
        return new TicketComment(content, createdByUserId, isInternal);
    }

    public void UpdateContent(string content)
    {
        if (DeletedAtUtc.HasValue)
            throw new InvalidOperationException("Cannot update a deleted comment.");

        if (Ticket.Status == TicketStatus.Closed)
            throw new InvalidOperationException("Cannot update a comment on a closed ticket.");

        Content = NormalizeContent(content);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Delete(int deletedByUserId)
    {
        if (DeletedAtUtc.HasValue)
            throw new InvalidOperationException("Comment is already deleted.");

        if (deletedByUserId <= 0)
            throw new ArgumentException(
                "User ID must be greater than zero.",
                nameof(deletedByUserId));

        DeletedAtUtc = DateTime.UtcNow;
        DeletedByUserId = deletedByUserId;
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
