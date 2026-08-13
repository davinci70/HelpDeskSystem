namespace HelpDesk.Application.Tickets.Commands.CloseTicket;

public class CloseTicketCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<CloseTicketCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result> Handle(CloseTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == request.TicketId, cancellationToken);

        if (ticket is null)
            return Result.Failure(TicketErrors.TicketNotFound);

        if (!ticket.Close(_currentUser.UserId!.Value))
            return Result.Failure(TicketErrors.CannotClose);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
