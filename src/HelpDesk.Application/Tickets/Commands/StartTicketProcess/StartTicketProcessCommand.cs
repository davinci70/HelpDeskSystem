namespace HelpDesk.Application.Tickets.Commands.StartTicketProcess;

public record StartTicketProcessCommand(
    int TicketId
) : ICommand;
