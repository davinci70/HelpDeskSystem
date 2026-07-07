using HelpDesk.Application.Errors;

namespace HelpDesk.Application.Tickets.Commands.UpdateTicket;

public class UpdateTicketCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateTicketCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (ticket is null)
            return Result.Failure(TicketErrors.TicketNotFound);

        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (category is null)
            return Result.Failure(TicketErrors.CategoryNotFound);

        ticket = request.Adapt(ticket);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
