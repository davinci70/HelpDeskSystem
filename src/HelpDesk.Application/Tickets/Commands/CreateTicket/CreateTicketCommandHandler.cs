using Microsoft.Extensions.Logging;

namespace HelpDesk.Application.Tickets.Commands.CreateTicket;

public sealed class CreateTicketCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService,
    ILogger<CreateTicketCommandHandler> logger)
    : IRequestHandler<CreateTicketCommand, Result<CreateTicketResponse>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly ILogger<CreateTicketCommandHandler> _logger = logger;

    public async Task<Result<CreateTicketResponse>> Handle(
        CreateTicketCommand request,
        CancellationToken cancellationToken)
    {
        var categoryExists = await _context.Categories
            .AnyAsync(
                x => x.Id == request.CategoryId,
                cancellationToken);

        if (!categoryExists)
        {
            return Result.Failure<CreateTicketResponse>(
                TicketErrors.CategoryNotFound);
        }

        var ticket = Ticket.Create(
            request.Title,
            request.Description,
            request.CategoryId,
            request.Priority,
            _currentUserService.UserId!.Value);

        _context.Tickets.Add(ticket);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(
            ticket.Adapt<CreateTicketResponse>());
    }
}
