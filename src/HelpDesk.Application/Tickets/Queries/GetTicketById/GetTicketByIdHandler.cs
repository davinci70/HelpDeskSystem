using HelpDesk.Application.Errors;

namespace HelpDesk.Application.Tickets.Queries.GetTicketById;

public class GetTicketByIdHandler(IApplicationDbContext context) : IRequestHandler<GetTicketByIdQuery, Result<GetTicketByIdResponse>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<GetTicketByIdResponse>> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
         var ticket = await _context.Tickets
            .AsNoTracking()
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (ticket is null)
            return Result.Failure<GetTicketByIdResponse>(TicketErrors.TicketNotFound);

        return Result.Success(ticket.Adapt<GetTicketByIdResponse>());
    }
}
