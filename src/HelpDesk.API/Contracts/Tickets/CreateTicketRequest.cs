namespace HelpDesk.API.Contracts.Tickets;

public sealed record CreateTicketRequest(
    string Title,
    string Description,
    int CategoryId,
    TicketPriority Priority 
);
