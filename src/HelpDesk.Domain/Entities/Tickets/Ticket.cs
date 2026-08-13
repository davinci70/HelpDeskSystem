namespace HelpDesk.Domain.Entities.Tickets;

public sealed class Ticket : AggregateRoot
{
    private readonly List<TicketComment> _comments = [];

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty; 
    public TicketStatus Status { get; private set; }
    public TicketPriority Priority { get; private set; }
    public int CategoryId { get; private set; }
    public int? AssigneeId { get; private set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    public int? AssignedByUserId { get; private set; }
    public int? ResolvedByUserId { get; private set; }
    public int? ClosedByUserId { get; private set; }
    public int? ReopenedByUserId { get; private set; }

    public User? AssignedByUser { get; private set; }
    public User? ResolvedByUser { get; private set; }
    public User? ClosedByUser { get; private set; }

    public Category Category { get; private set; } = null!;
    public User? Assignee { get; private set; } = null!;
    public User CreatedByUser { get; private set; } = null!;
    public ICollection<TicketHistory> History { get; private set; } = [];
    public IReadOnlyCollection<TicketComment> Comments => _comments.AsReadOnly();

    public void Assign(int userId, int? assignedByUserId)
    {
        AssigneeId = userId;
        AssignedByUserId = assignedByUserId;

        RaiseDomainEvent(
        new TicketAssignedDomainEvent(Id, userId));
    }
    
    public void Create()
    {
        RaiseDomainEvent(
       new TicketCreatedDomainEvent(this));
    }

    private bool _IsStatus(TicketStatus status) => Status == status;

    public bool StartProgress()
    {
        if (_IsStatus(TicketStatus.Open) && AssigneeId.HasValue)
        {
            Status = TicketStatus.InProgress;

            RaiseDomainEvent(new TicketStartedDomainEvent(Id));

            return true;
        }
        
        return false;
    }

    public bool Resolve(int? resolvedByUserId)
    {
        if (_IsStatus(TicketStatus.InProgress))
        {
            Status = TicketStatus.Resolved;
            ResolvedByUserId = resolvedByUserId;

            RaiseDomainEvent(new TicketResolvedDomainEvent(Id));

            return true;
        }
        return false;
    }

    public bool Close(int? closedByUserId)
    {
        if (_IsStatus(TicketStatus.Resolved))
        {
            Status = TicketStatus.Closed;
            ClosedByUserId = closedByUserId;

            RaiseDomainEvent(new TicketClosedDomainEvent(Id));

            return true;
        }
        return false;
    }

    public bool Reopen(int? reopenedByUserId)
    {
        if (_IsStatus(TicketStatus.Closed))
        {
            Status = TicketStatus.Open;
            ReopenedByUserId = reopenedByUserId;

            RaiseDomainEvent(new TicketReopenedDomainEvent(Id));

            return true;
        }
        return false;
    }






































    //private Ticket()
    //{
    //}

    //public Ticket(string title, string description, TicketPriority priority, int categoryId, int createdByUserId)
    //{
    //    SetTitle(title);
    //    SetDescription(description);
    //    SetPriority(priority);
    //    CreatedByUserId = createdByUserId;

    //    if (categoryId <= 0)
    //    {
    //        throw new ArgumentException("Category id must be greater than zero.", nameof(categoryId));
    //    }

    //    CategoryId = categoryId;
    //    Status = TicketStatus.Open;
    //    CreatedAtUtc = DateTime.UtcNow;
    //}

    //public void UpdateDetails(string title, string description, TicketPriority priority, int categoryId)
    //{
    //    SetTitle(title);
    //    SetDescription(description);
    //    SetPriority(priority);

    //    if (categoryId <= 0)
    //    {
    //        throw new ArgumentException("Category id must be greater than zero.", nameof(categoryId));
    //    }

    //    CategoryId = categoryId;
    //}

    //public void ChangeStatus(TicketStatus status)
    //{
    //    Status = status;
    //}

    //public void AddComment(TicketComment comment)
    //{
    //    if (comment is null)
    //    {
    //        throw new ArgumentNullException(nameof(comment));
    //    }

    //    _comments.Add(comment);
    //}

    //private void SetTitle(string title)
    //{
    //    if (string.IsNullOrWhiteSpace(title))
    //    {
    //        throw new ArgumentException("Ticket title cannot be empty.", nameof(title));
    //    }

    //    Title = title.Trim();
    //}

    //private void SetDescription(string description)
    //{
    //    if (string.IsNullOrWhiteSpace(description))
    //    {
    //        throw new ArgumentException("Ticket description cannot be empty.", nameof(description));
    //    }

    //    Description = description.Trim();
    //}

    //private void SetPriority(TicketPriority priority)
    //{
    //    if (!Enum.IsDefined(priority))
    //    {
    //        throw new ArgumentException("Invalid ticket priority.", nameof(priority));
    //    }

    //    Priority = priority;
    //}
}
