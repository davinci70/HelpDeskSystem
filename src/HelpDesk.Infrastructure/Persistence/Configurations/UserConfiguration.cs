namespace HelpDesk.Infrastructure.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(x => x.Email).IsUnique();
    }
}
