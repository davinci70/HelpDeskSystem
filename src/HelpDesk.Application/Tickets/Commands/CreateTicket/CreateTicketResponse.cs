namespace HelpDesk.Application.Tickets.Commands.CreateTicket;

public sealed record CreateTicketResponse(
    int Id,
    string Title,
    TicketStatus Status,
    DateTime CreatedAtUtc
);
