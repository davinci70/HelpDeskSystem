namespace HelpDesk.Domain.Entities.Tickets;

public sealed class TicketHistory : Entity
{
    private TicketHistory()
    {
    }

    private TicketHistory(
        int ticketId,
        TicketHistoryType type,
        string description,
        int? actionByUserId)
    {
        if (ticketId <= 0)
        {
            throw new ArgumentException(
                "Ticket ID must be greater than zero.",
                nameof(ticketId));
        }

        if (actionByUserId.HasValue && actionByUserId.Value <= 0)
        {
            throw new ArgumentException(
                "Action by user ID must be greater than zero.",
                nameof(actionByUserId));
        }

        TicketId = ticketId;
        Type = type;
        Description = description;
        ActionByUserId = actionByUserId;
        CreatedAt = DateTime.UtcNow;
    }

    public int TicketId { get; private set; }

    public TicketHistoryType Type { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public int? ActionByUserId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public Ticket Ticket { get; private set; } = null!;

    public static TicketHistory Created(
        int ticketId,
        int? actionByUserId)
    {
        return new TicketHistory(
            ticketId,
            TicketHistoryType.Created,
            "Ticket created.",
            actionByUserId);
    }

    public static TicketHistory Assigned(
        int ticketId,
        int userId,
        int? actionByUserId)
    {
        return new TicketHistory(
            ticketId,
            TicketHistoryType.Assigned,
            $"Assigned to user with Id = {userId}.",
            actionByUserId);
    }

    public static TicketHistory Started(
        int ticketId,
        int? actionByUserId)
    {
        return new TicketHistory(
            ticketId,
            TicketHistoryType.Started,
            "Work started.",
            actionByUserId);
    }

    public static TicketHistory Resolved(
        int ticketId,
        int? actionByUserId)
    {
        return new TicketHistory(
            ticketId,
            TicketHistoryType.Resolved,
            "Ticket resolved.",
            actionByUserId);
    }

    public static TicketHistory Closed(
        int ticketId,
        int? actionByUserId)
    {
        return new TicketHistory(
            ticketId,
            TicketHistoryType.Closed,
            "Ticket closed.",
            actionByUserId);
    }

    public static TicketHistory Reopened(
        int ticketId,
        int? actionByUserId)
    {
        return new TicketHistory(
            ticketId,
            TicketHistoryType.Reopened,
            "Ticket reopened.",
            actionByUserId);
    }
}
