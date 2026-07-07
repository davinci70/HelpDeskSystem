namespace HelpDesk.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<Ticket> Tickets { get; }
    DbSet<TicketComment> TicketComments { get; }
    DbSet<Agent> Agents { get; }
    DbSet<TicketHistory> TicketHistories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
