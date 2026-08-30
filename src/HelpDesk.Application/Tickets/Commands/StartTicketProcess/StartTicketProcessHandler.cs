namespace HelpDesk.Application.Tickets.Commands.StartTicketProcess;

public sealed class StartTicketProcessHandler(IApplicationDbContext context, ICurrentUserService _currentUser)
    : IRequestHandler<StartTicketProcessCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUser = _currentUser;

    public async Task<Result> Handle(StartTicketProcessCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == request.TicketId, cancellationToken);

        if (ticket is null)
            return Result.Failure(TicketErrors.TicketNotFound);

        var transitionResult = ticket.StartProgress(_currentUser.UserId!.Value);

        if (transitionResult == TicketTransitionResult.InvalidStatus)
            return Result.Failure(TicketErrors.CannotStart);

        if (transitionResult == TicketTransitionResult.NotAssigned)
            return Result.Failure(TicketErrors.NotAssigned);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
