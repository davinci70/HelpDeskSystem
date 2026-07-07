namespace HelpDesk.Application.Tickets.Commands.ResolveTicket;

public class ResolveTicketCommandHandler(IApplicationDbContext context) : IRequestHandler<ResolveTicketCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(ResolveTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket =  await _context.Tickets.FirstOrDefaultAsync(x => x.Id == request.TicketId, cancellationToken);

        if (ticket is null)
            return Result.Failure(TicketErrors.TicketNotFound);

        if (!ticket.Resolve())
            return Result.Failure(TicketErrors.CannotResolve);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(ticket);
    }
}
