namespace HelpDesk.Application.Tickets.Commands.ResolveTicket;

public record ResolveTicketCommand(int TicketId) : ICommand;
