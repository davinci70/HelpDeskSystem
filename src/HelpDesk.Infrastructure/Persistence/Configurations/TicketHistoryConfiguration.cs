namespace HelpDesk.Infrastructure.Persistence.Configurations;

internal class TicketHistoryConfiguration : IEntityTypeConfiguration<TicketHistory>
{
    public void Configure(EntityTypeBuilder<TicketHistory> builder)
    {
        builder.Property(x => x.Type).IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();
    }
}
