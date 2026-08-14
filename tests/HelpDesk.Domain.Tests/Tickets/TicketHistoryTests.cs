using FluentAssertions;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;

namespace HelpDesk.Domain.UnitTests.Entities.Tickets;

public sealed class TicketHistoryTests
{
    private const int TicketId = 10;
    private const int ActionByUserId = 20;

    [Fact]
    public void Created_ShouldCreateCorrectHistory()
    {
        // Act
        var history = TicketHistory.Created(
            TicketId,
            ActionByUserId);

        // Assert
        history.TicketId.Should().Be(TicketId);
        history.Type.Should().Be(TicketHistoryType.Created);
        history.Description.Should().Be("Ticket created.");
        history.ActionByUserId.Should().Be(ActionByUserId);

        history.CreatedAt.Should().BeCloseTo(
            DateTime.UtcNow,
            TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Created_ShouldAllowNullActionByUserId()
    {
        // Act
        var history = TicketHistory.Created(
            TicketId,
            null);

        // Assert
        history.ActionByUserId.Should().BeNull();
    }

    [Fact]
    public void Assigned_ShouldCreateCorrectHistory()
    {
        // Act
        var history = TicketHistory.Assigned(
            TicketId,
            30,
            ActionByUserId);

        // Assert
        history.TicketId.Should().Be(TicketId);
        history.Type.Should().Be(TicketHistoryType.Assigned);
        history.Description.Should().Be(
            "Assigned to user with Id = 30.");
        history.ActionByUserId.Should().Be(ActionByUserId);
    }

    [Fact]
    public void Started_ShouldCreateCorrectHistory()
    {
        // Act
        var history = TicketHistory.Started(
            TicketId,
            ActionByUserId);

        // Assert
        history.TicketId.Should().Be(TicketId);
        history.Type.Should().Be(TicketHistoryType.Started);
        history.Description.Should().Be("Work started.");
        history.ActionByUserId.Should().Be(ActionByUserId);
    }

    [Fact]
    public void Resolved_ShouldCreateCorrectHistory()
    {
        // Act
        var history = TicketHistory.Resolved(
            TicketId,
            ActionByUserId);

        // Assert
        history.TicketId.Should().Be(TicketId);
        history.Type.Should().Be(TicketHistoryType.Resolved);
        history.Description.Should().Be("Ticket resolved.");
        history.ActionByUserId.Should().Be(ActionByUserId);
    }

    [Fact]
    public void Closed_ShouldCreateCorrectHistory()
    {
        // Act
        var history = TicketHistory.Closed(
            TicketId,
            ActionByUserId);

        // Assert
        history.TicketId.Should().Be(TicketId);
        history.Type.Should().Be(TicketHistoryType.Closed);
        history.Description.Should().Be("Ticket closed.");
        history.ActionByUserId.Should().Be(ActionByUserId);
    }

    [Fact]
    public void Reopened_ShouldCreateCorrectHistory()
    {
        // Act
        var history = TicketHistory.Reopened(
            TicketId,
            ActionByUserId);

        // Assert
        history.TicketId.Should().Be(TicketId);
        history.Type.Should().Be(TicketHistoryType.Reopened);
        history.Description.Should().Be("Ticket reopened.");
        history.ActionByUserId.Should().Be(ActionByUserId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Created_ShouldThrow_WhenTicketIdIsInvalid(
        int ticketId)
    {
        // Act
        var act = () => TicketHistory.Created(
            ticketId,
            ActionByUserId);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("ticketId");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Created_ShouldThrow_WhenActionByUserIdIsInvalid(
        int actionByUserId)
    {
        // Act
        var act = () => TicketHistory.Created(
            TicketId,
            actionByUserId);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("actionByUserId");
    }
}