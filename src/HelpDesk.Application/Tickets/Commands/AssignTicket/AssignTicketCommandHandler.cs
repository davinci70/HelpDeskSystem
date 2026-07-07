namespace HelpDesk.Application.Tickets.Commands.AssignTicket;

public class AssignTicketCommandHandler(IApplicationDbContext context) : IRequestHandler<AssignTicketCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(AssignTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == request.TicketId, cancellationToken);

        if (ticket is null)
            return Result.Failure(TicketErrors.TicketNotFound);

        var agent = await _context.Agents.FirstOrDefaultAsync(a => a.Id == request.AgentId, cancellationToken);

        if (agent is null)
            return Result.Failure(AgentErrors.AgentNotFound);

        if (!agent.IsActive)
            return Result.Failure(AgentErrors.AgentNotActive);

        ticket.Assign(request.AgentId);

        await _context.TicketHistories.AddAsync(TicketHistory.Assigned(ticket.Id, agent.Name), cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
