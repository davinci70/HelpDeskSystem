using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HelpDesk.Infrastructure.Persistence.Configurations;

public class AgentConfiguration : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        builder.Property(agent => agent.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(agent => agent.Email)
            .IsRequired()
            .HasMaxLength(100);
    }
}
