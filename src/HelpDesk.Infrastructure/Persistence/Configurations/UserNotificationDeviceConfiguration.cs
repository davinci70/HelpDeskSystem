namespace HelpDesk.Infrastructure.Persistence.Configurations;

internal class UserNotificationDeviceConfiguration : IEntityTypeConfiguration<UserNotificationDevice>
{
    public void Configure(EntityTypeBuilder<UserNotificationDevice> builder)
    {
        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.FcmToken)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.LastSeenAtUtc)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.NotificationDevices)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.FcmToken)
            .IsUnique();

        builder.HasIndex(x => new
        {
            x.UserId,
            x.IsActive
        });
    }
}
