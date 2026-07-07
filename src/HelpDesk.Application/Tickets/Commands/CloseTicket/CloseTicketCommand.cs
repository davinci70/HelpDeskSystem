namespace HelpDesk.Application.Tickets.Commands.CloseTicket;

public record CloseTicketCommand(int TicketId) : ICommand;
