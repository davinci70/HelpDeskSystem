namespace HelpDesk.API.Contracts.Tickets;

public record UpdateTicketRequest(
    string Title,
    string Description,
    TicketPriority Priority,
    int CategoryId
);