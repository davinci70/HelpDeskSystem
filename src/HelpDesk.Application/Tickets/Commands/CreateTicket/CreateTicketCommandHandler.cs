using HelpDesk.Application.Abstractions.Persistence;
using HelpDesk.Application.Categories.Commands.CreateCategory;
using HelpDesk.Application.Errors;

namespace HelpDesk.Application.Tickets.Commands.CreateTicket;

public sealed class CreateTicketCommandHandler(IApplicationDbContext context)
        : IRequestHandler<CreateTicketCommand, Result<CreateTicketResponse>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<CreateTicketResponse>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var categoryExists = await _context.Categories.AnyAsync(x => x.Id == request.CategoryId, cancellationToken);

        if (!categoryExists)
            return Result.Failure<CreateTicketResponse>(TicketErrors.CategoryNotFound);

        var ticket = request.Adapt<Ticket>();

        _context.Tickets.Add(ticket);

        await _context.SaveChangesAsync(cancellationToken);

        var history = TicketHistory.Created(ticket.Id);

        await _context.TicketHistories.AddAsync(history, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(ticket.Adapt<CreateTicketResponse>());
    }
}
