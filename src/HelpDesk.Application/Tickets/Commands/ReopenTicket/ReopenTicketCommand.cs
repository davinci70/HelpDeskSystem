namespace HelpDesk.Application.Tickets.Commands.ReopenTicket;

public record ReopenTicketCommand(int TicketId) : ICommand;
