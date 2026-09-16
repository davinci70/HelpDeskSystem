using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.Infrastructure.Persistence.Configurations;

public sealed class TicketCommentConfiguration : IEntityTypeConfiguration<TicketComment>
{
    public void Configure(EntityTypeBuilder<TicketComment> builder)
    {
        builder.Property(comment => comment.CreatedByUserId)
            .IsRequired();
        
        builder.Property(comment => comment.DeletedByUserId)
            .IsRequired(false);
        
        builder.Property(comment => comment.IsInternal)
            .IsRequired();

        builder.Property(comment => comment.UpdatedAtUtc)
            .IsRequired(false);
        
        builder.Property(comment => comment.DeletedAtUtc)
            .IsRequired(false);

        builder.Property(comment => comment.Content)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(comment => comment.CreatedAtUtc)
            .IsRequired();

        builder.HasOne(comment => comment.Ticket)
            .WithMany(ticket => ticket.Comments)
            .HasForeignKey(comment => comment.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(comment => comment.CreatedByUser)
            .WithMany(user => user.CreatedTicketComments)
            .HasForeignKey(comment => comment.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(comment => comment.DeletedByUser)
            .WithMany(user => user.DeletedTicketComments)
            .HasForeignKey(comment => comment.DeletedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

    }
}
