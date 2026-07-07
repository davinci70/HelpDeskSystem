namespace HelpDesk.Application.Tickets.Commands.ReopenTicket;

public class ReopenTicketCommandHandler(IApplicationDbContext context) : IRequestHandler<ReopenTicketCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(ReopenTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == request.TicketId, cancellationToken);

        if (ticket is null)
            return Result.Failure(TicketErrors.TicketNotFound);

        if (!ticket.Reopen())
            return Result.Failure(TicketErrors.CannotReopen);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
