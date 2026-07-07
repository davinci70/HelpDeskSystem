namespace HelpDesk.Application.Tickets.Commands.AssignTicket;

public record AssignTicketCommand(
    int TicketId,
    int AgentId
) : ICommand;
