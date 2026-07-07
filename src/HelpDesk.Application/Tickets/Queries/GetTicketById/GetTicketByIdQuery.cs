namespace HelpDesk.Application.Tickets.Queries.GetTicketById;

public record GetTicketByIdQuery(int Id) : IQuery<GetTicketByIdResponse>;
