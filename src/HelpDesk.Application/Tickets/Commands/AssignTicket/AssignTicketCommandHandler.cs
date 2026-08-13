namespace HelpDesk.Application.Tickets.Commands.AssignTicket;

public class AssignTicketCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<AssignTicketCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUser = currentUser;

    public async Task<Result> Handle(AssignTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == request.TicketId, cancellationToken);

        if (ticket is null)
            return Result.Failure(TicketErrors.TicketNotFound);

        var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == request.UserId, cancellationToken);

        if (user is null)
            return Result.Failure(AgentErrors.AgentNotFound);

        if (user.Role != UserRole.Agent)
        {
            return Result.Failure(
                TicketErrors.UserIsNotAnAgent);
        }

        ticket.Assign(request.UserId, _currentUser.UserId!.Value);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
