namespace HelpDesk.Application.Tickets.Queries.GetTickets;

public record GetTicketSummaryResponse(
    int Id,
    string Title,
    TicketPriority Priority,
    TicketStatus Status,
    DateTime CreatedAtUtc,
    string CategoryName
);
