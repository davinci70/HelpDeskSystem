namespace HelpDesk.Application.Tickets.Commands.UpdateTicket;

public record UpdateTicketCommand(
    int Id,
    string Title,
    string Description,
    TicketPriority Priority,
    int CategoryId
) : ICommand;
