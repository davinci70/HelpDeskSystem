namespace HelpDesk.Domain.Common.DomainEvents;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
