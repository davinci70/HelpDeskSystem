namespace HelpDesk.Domain.Entities;

public class TicketHistory
{
    public int Id { get; private set; }
    public int TicketId { get; private set; }
    public TicketHistoryType Type { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public Ticket Ticket { get; private set; } = default!;

    private TicketHistory()
    {
    }

    private TicketHistory(
        int ticketId,
        TicketHistoryType type,
        string description)
    {
        TicketId = ticketId;
        Type = type;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public static TicketHistory Created(int ticketId)
    {
        return new TicketHistory(
            ticketId,
            TicketHistoryType.Created,
            "Ticket created.");
    }

    public static TicketHistory Assigned(int ticketId, string agentName)
    {
        return new TicketHistory(
            ticketId,
            TicketHistoryType.Assigned,
            $"Assigned to {agentName}.");
    }

    public static TicketHistory Started(int ticketId)
    {
        return new TicketHistory(
            ticketId,
            TicketHistoryType.Started,
            "Work started.");
    }

    public static TicketHistory Resolved(int ticketId)
    {
        return new TicketHistory(
            ticketId,
            TicketHistoryType.Resolved,
            "Ticket resolved.");
    }

    public static TicketHistory Closed(int ticketId)
    {
        return new TicketHistory(
            ticketId,
            TicketHistoryType.Closed,
            "Ticket closed.");
    }

    public static TicketHistory Reopened(int ticketId)
    {
        return new TicketHistory(
            ticketId,
            TicketHistoryType.Reopened,
            "Ticket reopened.");
    }
}
