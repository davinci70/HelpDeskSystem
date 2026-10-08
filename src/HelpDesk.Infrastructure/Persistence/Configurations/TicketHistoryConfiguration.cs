namespace HelpDesk.Infrastructure.Persistence.Configurations;

internal class TicketHistoryConfiguration : IEntityTypeConfiguration<TicketHistory>
{
    public void Configure(EntityTypeBuilder<TicketHistory> builder)
    {
        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.TicketCommentId)
            .IsRequired(false);

        builder.HasOne(x => x.Ticket)
            .WithMany()
            .HasForeignKey(x => x.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.ActionByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<TicketComment>()
            .WithMany()
            .HasForeignKey(x => x.TicketCommentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
