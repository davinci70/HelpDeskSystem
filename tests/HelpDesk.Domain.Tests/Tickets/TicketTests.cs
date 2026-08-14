using FluentAssertions;
using HelpDesk.Domain.Entities.Tickets;
using HelpDesk.Domain.Entities.Tickets.Events;
using HelpDesk.Domain.Enums;

namespace HelpDesk.Domain.Tests.Tickets;

public class TicketTests
{
    private const int CategoryId = 1;
    private const int CreatedByUserId = 10;
    private const int AgentId = 20;
    private const int ActionByUserId = 30;

    private static Ticket CreateTicket(TicketPriority priority = TicketPriority.Medium)
    {
        return Ticket.Create("Sample Title", "Sample Description", CategoryId, priority, CreatedByUserId);  
    }

    [Fact]
    public void Create_ShouldCreateTicketWithExpectedValues()
    {
        // act
        var ticket = CreateTicket();

        // assert
        ticket.Title.Should().Be("Sample Title");
        ticket.Description.Should().Be("Sample Description");
        ticket.CategoryId.Should().Be(CategoryId);
        ticket.Priority.Should().Be(TicketPriority.Medium);
        ticket.CreatedByUserId.Should().Be(CreatedByUserId);
        ticket.Status.Should().Be(TicketStatus.Open);
        ticket.AssigneeId.Should().BeNull();
        ticket.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_ShouldRaiseTicketCreatedDomainEvent()
    {
        // act
        var ticket = CreateTicket();

        // assert
        ticket.DomainEvents.Should().ContainSingle();

        var domainEvent = ticket.DomainEvents.Single();

        domainEvent.Should().BeOfType<TicketCreatedDomainEvent>();

        var CreatedEvent = (TicketCreatedDomainEvent)domainEvent;

        CreatedEvent.Ticket.Should().BeSameAs(ticket);

        CreatedEvent.OccurredOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_ShouldThrow_WhenTitleIsEmpty(string title)
    {
        // Act
        var act = () => Ticket.Create(
            title,
            "Valid description",
            CategoryId,
            TicketPriority.Medium,
            CreatedByUserId);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("title");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_ShouldThrow_WhenDescriptionIsEmpty(string description)
    {
        // Act
        var act = () => Ticket.Create(
            "Valid title",
            description,
            CategoryId,
            TicketPriority.Medium,
            CreatedByUserId);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("description");
    }

    [Fact]
    public void Create_ShouldThrow_WhenCategoryIdIsInvalid()
    {
        // Act
        var act = () => Ticket.Create(
            "Valid title",
            "Valid description",
            0,
            TicketPriority.Medium,
            CreatedByUserId);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("categoryId");
    }

    [Fact]
    public void Create_ShouldThrow_WhenCreatedByUserIdIsInvalid()
    {
        // Act
        var act = () => Ticket.Create(
            "Valid title",
            "Valid description",
            CategoryId,
            TicketPriority.Medium,
            0);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("createdByUserId");
    }

    [Fact]
    public void Create_ShouldTrimTitleAndDescription()
    {
        // Act
        var ticket = Ticket.Create(
            "  Test ticket  ",
            "  Test description  ",
            CategoryId,
            TicketPriority.High,
            CreatedByUserId);

        // Assert
        ticket.Title.Should().Be("Test ticket");
        ticket.Description.Should().Be("Test description");
    }

    [Fact]
    public void Assign_ShouldSetAssigneeAndRaiseDomainEvent()
    {
        // Arrange
        var ticket = CreateTicket();
        ticket.ClearDomainEvents();

        // Act
        ticket.Assign(AgentId, ActionByUserId);

        // Assert
        ticket.AssigneeId.Should().Be(AgentId);

        ticket.DomainEvents.Should().ContainSingle();

        var domainEvent = ticket.DomainEvents.Single()
            .Should()
            .BeOfType<TicketAssignedDomainEvent>()
            .Subject;

        domainEvent.TicketId.Should().Be(ticket.Id);
        domainEvent.UserId.Should().Be(AgentId);
    }

    [Fact]
    public void Assign_ShouldThrow_WhenUserIdIsInvalid()
    {
        // Arrange
        var ticket = CreateTicket();
        ticket.ClearDomainEvents();

        // Act
        var act = () => ticket.Assign(0, ActionByUserId);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("userId");
    }

    [Fact]
    public void Assign_ShouldThrow_WhenAssignedByUserIdIsInvalid()
    {
        // Arrange
        var ticket = CreateTicket();
        ticket.ClearDomainEvents();

        // Act
        var act = () => ticket.Assign(
            AgentId,
            0);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("actionByUserId");
    }

    [Fact]
    public void StartProgress_ShouldReturnNotAssigned_WhenTicketHasNoAssignee()
    {
        // Arrange
        var ticket = CreateTicket();
        ticket.ClearDomainEvents();

        // Act
        var result = ticket.StartProgress(ActionByUserId);

        // Assert
        result.Should().Be(TicketTransitionResult.NotAssigned);
        ticket.Status.Should().Be(TicketStatus.Open);
        ticket.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void StartProgress_ShouldReturnInvalidStatus_WhenTicketIsNotOpen()
    {
        // Arrange
        var ticket = CreateTicket();
        ticket.Assign(AgentId, ActionByUserId);
        ticket.ClearDomainEvents();

        ticket.StartProgress(ActionByUserId);
        ticket.ClearDomainEvents();

        // Act
        var result = ticket.StartProgress(ActionByUserId);

        // Assert
        result.Should().Be(TicketTransitionResult.InvalidStatus);
        ticket.Status.Should().Be(TicketStatus.InProgress);
        ticket.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void StartProgress_ShouldChangeStatusAndRaiseDomainEvent()
    {
        // Arrange
        var ticket = CreateTicket();

        ticket.Assign(AgentId, ActionByUserId);
        ticket.ClearDomainEvents();

        // Act
        var result = ticket.StartProgress(ActionByUserId);

        // Assert
        result.Should().Be(TicketTransitionResult.Success);
        ticket.Status.Should().Be(TicketStatus.InProgress);

        ticket.DomainEvents.Should().ContainSingle();

        var domainEvent = ticket.DomainEvents.Single()
            .Should()
            .BeOfType<TicketStartedDomainEvent>()
            .Subject;

        domainEvent.TicketId.Should().Be(ticket.Id);
    }

    [Fact]
    public void Resolve_ShouldReturnInvalidStatus_WhenTicketIsNotInProgress()
    {
        // Arrange
        var ticket = CreateTicket();
        ticket.ClearDomainEvents();

        // Act
        var result = ticket.Resolve(ActionByUserId);

        // Assert
        result.Should().Be(TicketTransitionResult.InvalidStatus);
        ticket.Status.Should().Be(TicketStatus.Open);
        ticket.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Resolve_ShouldThrow_WhenResolvedByUserIdIsInvalid()
    {
        // Arrange
        var ticket = CreateTicket();

        ticket.Assign(AgentId, ActionByUserId);
        ticket.ClearDomainEvents();

        ticket.StartProgress(ActionByUserId);
        ticket.ClearDomainEvents();

        // Act
        var act = () => ticket.Resolve(0);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("actionByUserId");

        ticket.Status.Should().Be(TicketStatus.InProgress);
        ticket.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Resolve_ShouldChangeStatusAndRaiseDomainEvent()
    {
        // Arrange
        var ticket = CreateTicket();

        ticket.Assign(AgentId, ActionByUserId);
        ticket.ClearDomainEvents();

        ticket.StartProgress(ActionByUserId);
        ticket.ClearDomainEvents();

        // Act
        var result = ticket.Resolve(ActionByUserId);

        // Assert
        result.Should().Be(TicketTransitionResult.Success);
        ticket.Status.Should().Be(TicketStatus.Resolved);

        ticket.DomainEvents.Should().ContainSingle();

        ticket.DomainEvents.Single()
            .Should()
            .BeOfType<TicketResolvedDomainEvent>();
    }

    [Fact]
    public void Close_ShouldReturnInvalidStatus_WhenTicketIsNotResolved()
    {
        // Arrange
        var ticket = CreateTicket();
        ticket.ClearDomainEvents();

        // Act
        var result = ticket.Close(ActionByUserId);

        // Assert
        result.Should().Be(TicketTransitionResult.InvalidStatus);
        ticket.Status.Should().Be(TicketStatus.Open);
        ticket.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Close_ShouldChangeStatusAndRaiseDomainEvent()
    {
        // Arrange
        var ticket = CreateTicket();

        ticket.Assign(AgentId, ActionByUserId);
        ticket.ClearDomainEvents();

        ticket.StartProgress(ActionByUserId);
        ticket.ClearDomainEvents();

        ticket.Resolve(ActionByUserId);
        ticket.ClearDomainEvents();

        // Act
        var result = ticket.Close(ActionByUserId);

        // Assert
        result.Should().Be(TicketTransitionResult.Success);
        ticket.Status.Should().Be(TicketStatus.Closed);

        ticket.DomainEvents.Should().ContainSingle();

        ticket.DomainEvents.Single()
            .Should()
            .BeOfType<TicketClosedDomainEvent>();
    }

    [Fact]
    public void Reopen_ShouldReturnInvalidStatus_WhenTicketIsNotClosed()
    {
        // Arrange
        var ticket = CreateTicket();
        ticket.ClearDomainEvents();

        // Act
        var result = ticket.Reopen(ActionByUserId);

        // Assert
        result.Should().Be(TicketTransitionResult.InvalidStatus);
        ticket.Status.Should().Be(TicketStatus.Open);
        ticket.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Reopen_ShouldChangeStatusAndRaiseDomainEvent()
    {
        // Arrange
        var ticket = CreateTicket();

        ticket.Assign(AgentId, ActionByUserId);
        ticket.ClearDomainEvents();

        ticket.StartProgress(ActionByUserId);
        ticket.ClearDomainEvents();

        ticket.Resolve(ActionByUserId);
        ticket.ClearDomainEvents();

        ticket.Close(ActionByUserId);
        ticket.ClearDomainEvents();

        // Act
        var result = ticket.Reopen(ActionByUserId);

        // Assert
        result.Should().Be(TicketTransitionResult.Success);
        ticket.Status.Should().Be(TicketStatus.Open);

        ticket.DomainEvents.Should().ContainSingle();

        ticket.DomainEvents.Single()
            .Should()
            .BeOfType<TicketReopenedDomainEvent>();
    }

    [Fact]
    public void AddComment_ShouldAddCommentToTicket()
    {
        // Arrange
        var ticket = CreateTicket();
        const string content = "This is a test comment.";

        // Act
        var comment = ticket.AddComment(content);

        // Assert
        comment.Should().NotBeNull();
        comment.Content.Should().Be(content);

        ticket.Comments.Should().ContainSingle();
        ticket.Comments.Single().Should().BeSameAs(comment);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        // Arrange
        var ticket = CreateTicket();

        ticket.DomainEvents.Should().ContainSingle();

        // Act
        ticket.ClearDomainEvents();

        // Assert
        ticket.DomainEvents.Should().BeEmpty();
    }
}
