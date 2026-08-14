using HelpDesk.Domain.Entities.Tickets;

namespace HelpDesk.Application.Mapping;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Ticket, GetTicketSummaryResponse>()
            .Map(dest => dest.CategoryName, src => src.Category.Name);
    }
}
