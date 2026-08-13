using HelpDesk.Domain.Entities.Tickets;

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

        builder.HasOne(x => x.Assignee)
            .WithMany()
            .HasForeignKey(x => x.AssigneeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Metadata
            .FindNavigation(nameof(Ticket.Comments))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(t => t.AssignedByUser)
            .WithMany()
            .HasForeignKey(t => t.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ResolvedByUser)
            .WithMany()
            .HasForeignKey(t => t.ResolvedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ClosedByUser)
            .WithMany()
            .HasForeignKey(t => t.ClosedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
