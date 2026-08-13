namespace HelpDesk.Application.Tickets.Commands.ReopenTicket;

public class ReopenTicketCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<ReopenTicketCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result> Handle(ReopenTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == request.TicketId, cancellationToken);

        if (ticket is null)
            return Result.Failure(TicketErrors.TicketNotFound);

        if (!ticket.Reopen(_currentUser.UserId!.Value))
            return Result.Failure(TicketErrors.CannotReopen);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
