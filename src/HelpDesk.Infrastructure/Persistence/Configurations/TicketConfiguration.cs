namespace HelpDesk.Infrastructure.Persistence.Configurations;

public sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.Property(ticket => ticket.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ticket => ticket.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(ticket => ticket.Status)
            .IsRequired();

        builder.Property(ticket => ticket.Priority)
            .IsRequired();

        builder.Property(ticket => ticket.CreatedAtUtc)
            .IsRequired();

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Metadata
            .FindNavigation(nameof(Ticket.Comments))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
