namespace HelpDesk.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<Ticket> Tickets { get; }
    DbSet<TicketComment> TicketComments { get; }
    DbSet<TicketHistory> TicketHistories { get; }
    DbSet<User> Users { get; }
    DbSet<UserNotificationDevice> UserNotificationDevices { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
