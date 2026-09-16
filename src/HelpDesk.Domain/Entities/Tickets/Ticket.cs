namespace HelpDesk.Domain.Entities.Tickets;

public sealed class Ticket : AggregateRoot
{
    private readonly List<TicketComment> _comments = [];

    private Ticket()
    {
    }

    private Ticket(
        string title,
        string description,
        int categoryId,
        TicketPriority priority,
        int createdByUserId)
    {
        SetTitle(title);
        SetDescription(description);

        if (categoryId <= 0)
            throw new ArgumentException(
                "Category ID must be greater than zero.",
                nameof(categoryId));

        if (createdByUserId <= 0)
            throw new ArgumentException(
                "User ID must be greater than zero.",
                nameof(createdByUserId));

        CategoryId = categoryId;
        Priority = priority;
        CreatedByUserId = createdByUserId;
        Status = TicketStatus.Open;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public string Title { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public TicketStatus Status { get; private set; }

    public TicketPriority Priority { get; private set; }

    public int CategoryId { get; private set; }

    public int? AssigneeId { get; private set; }

    public int CreatedByUserId { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public Category Category { get; private set; } = null!;

    public User? Assignee { get; private set; }

    public User CreatedByUser { get; private set; } = null!;

    public IReadOnlyCollection<TicketComment> Comments =>
        _comments.AsReadOnly();

    public TicketComment AddComment(string content, int createdByUserId, bool isInternal)
    {
        if (Status == TicketStatus.Closed)
            throw new InvalidOperationException("Cannot add a comment to a closed ticket.");

        var comment = TicketComment.Create(content, createdByUserId, isInternal);

        _comments.Add(comment);

        return comment;
    }

    public static Ticket Create(
        string title,
        string description,
        int categoryId,
        TicketPriority priority,
        int createdByUserId)
    {
        var ticket = new Ticket(
            title,
            description,
            categoryId,
            priority,
            createdByUserId);

        ticket.RaiseDomainEvent(
            new TicketCreatedDomainEvent(ticket));

        return ticket;
    }

    public void Assign(int userId, int? actionByUserId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException(
                "User ID must be greater than zero.",
                nameof(userId));
        }

        ValidateOptionalUserId(actionByUserId, nameof(actionByUserId));

        AssigneeId = userId;

        RaiseDomainEvent(new TicketAssignedDomainEvent(Id, userId, actionByUserId));
    }

    public TicketTransitionResult StartProgress(int? actionByUserId)
    {
        if (Status != TicketStatus.Open)
            return TicketTransitionResult.InvalidStatus;

        if (!AssigneeId.HasValue)
            return TicketTransitionResult.NotAssigned;

        ValidateOptionalUserId(actionByUserId, nameof(actionByUserId));

        Status = TicketStatus.InProgress;

        RaiseDomainEvent(new TicketStartedDomainEvent(Id, actionByUserId));

        return TicketTransitionResult.Success;
    }

    public TicketTransitionResult Resolve(int? actionByUserId)
    {
        if (Status != TicketStatus.InProgress)
            return TicketTransitionResult.InvalidStatus;

        ValidateOptionalUserId(actionByUserId, nameof(actionByUserId));

        Status = TicketStatus.Resolved;

        RaiseDomainEvent(new TicketResolvedDomainEvent(Id, actionByUserId));

        return TicketTransitionResult.Success;
    }

    public TicketTransitionResult Close(int? actionByUserId)
    {
        if (Status != TicketStatus.Resolved)
            return TicketTransitionResult.InvalidStatus;

        ValidateOptionalUserId(actionByUserId, nameof(actionByUserId));

        Status = TicketStatus.Closed;

        RaiseDomainEvent(new TicketClosedDomainEvent(Id, actionByUserId));

        return TicketTransitionResult.Success;
    }

    public TicketTransitionResult Reopen(int? actionByUserId)
    {
        if (Status != TicketStatus.Closed)
            return TicketTransitionResult.InvalidStatus;

        ValidateOptionalUserId(actionByUserId, nameof(actionByUserId));

        Status = TicketStatus.Open;

        RaiseDomainEvent(new TicketReopenedDomainEvent(Id, actionByUserId));

        return TicketTransitionResult.Success;
    }

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Title cannot be empty.",
                nameof(title));
        }

        Title = title.Trim();
    }

    private void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(
                "Description cannot be empty.",
                nameof(description));
        }

        Description = description.Trim();
    }

    private static void ValidateOptionalUserId(
        int? userId,
        string parameterName)
    {
        if (userId.HasValue && userId.Value <= 0)
        {
            throw new ArgumentException(
                "User Id must be greater than zero.",
                parameterName);
        }
    }
}