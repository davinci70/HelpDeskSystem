namespace HelpDesk.Application.Tickets.Commands.StartTicketProcess;

public class StartTicketProcessHandler(IApplicationDbContext context) : IRequestHandler<StartTicketProcessCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(StartTicketProcessCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == request.TicketId, cancellationToken);

        if (ticket is null)
            return Result.Failure(TicketErrors.TicketNotFound);

        if (ticket.AssigneeId is null)
            return Result.Failure(TicketErrors.NotAssigned);

        if (!ticket.StartProgress())
            return Result.Failure(TicketErrors.CannotStart);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
