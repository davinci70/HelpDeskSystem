namespace HelpDesk.Application.Tickets.Commands.CreateTicket;

public sealed record CreateTicketCommand(
    string Title,
    string Description,
    int CategoryId,
    TicketPriority Priority
) : ICommand<CreateTicketResponse>; 
