using HelpDesk.Domain.Entities.Tickets;

namespace HelpDesk.Application.Mapping;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateTicketCommand, Ticket>()
            .Map(dest => dest.Status, src => TicketStatus.Open)
            .Map(dest => dest.CreatedAtUtc, src => DateTime.UtcNow);

        config.NewConfig<Ticket, GetTicketSummaryResponse>()
            .Map(dest => dest.CategoryName, src => src.Category.Name);
    }
}
