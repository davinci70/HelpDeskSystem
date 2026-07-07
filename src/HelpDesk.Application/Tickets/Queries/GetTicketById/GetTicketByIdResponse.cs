namespace HelpDesk.Application.Tickets.Queries.GetTicketById;

public record GetTicketByIdResponse(
    int Id,
    string Title,
    string Description,
    TicketPriority Priority,
    TicketStatus Status,
    DateTime CreatedAtUtc,
    CategoryResponse Category
);
