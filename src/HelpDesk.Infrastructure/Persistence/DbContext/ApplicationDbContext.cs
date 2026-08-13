namespace HelpDesk.Infrastructure.Persistence.DbContext;

public sealed class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext, IApplicationDbContext
{
    private readonly IDomainEventDispatcher _dispatcher;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventDispatcher dispatcher) : base(options)
    {
        _dispatcher = dispatcher;
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<TicketComment> TicketComments => Set<TicketComment>();
    public DbSet<TicketHistory> TicketHistories => Set<TicketHistory>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        // First save
        var result = await base.SaveChangesAsync(cancellationToken);

        // Collect aggregates that raised events
        var aggregates = ChangeTracker
            .Entries<AggregateRoot>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count != 0)
            .ToList();

        if (!aggregates.Any())
        {
            return result;
        }

        // Copy events
        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        // Clear BEFORE dispatch to avoid recursion
        foreach (var aggregate in aggregates)
        {
            aggregate.ClearDomainEvents();
        }

        // Publish
        await _dispatcher.DispatchAsync(domainEvents, cancellationToken);

        // Persist anything handlers added
        await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
