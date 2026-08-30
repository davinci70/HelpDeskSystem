using HelpDesk.Application.Abstractions.Persistence;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Entities.Notifications;
using HelpDesk.Domain.Entities.Tickets;
using HelpDesk.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Application.Tests.Common;

public sealed class TestApplicationDbContext : DbContext, IApplicationDbContext
{
    public TestApplicationDbContext(
        DbContextOptions<TestApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<TicketComment> TicketComments => Set<TicketComment>();

    public DbSet<TicketHistory> TicketHistories => Set<TicketHistory>();

    public DbSet<User> Users => Set<User>();

    public DbSet<UserNotificationDevice> UserNotificationDevices => Set<UserNotificationDevice>();

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
