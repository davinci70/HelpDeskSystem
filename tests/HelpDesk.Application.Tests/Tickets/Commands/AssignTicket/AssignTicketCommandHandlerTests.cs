using FluentAssertions;
using HelpDesk.Application.Common.Interfaces;
using HelpDesk.Application.Errors;
using HelpDesk.Application.Tests.Common;
using HelpDesk.Application.Tickets.Commands.AssignTicket;
using HelpDesk.Domain.Entities.Tickets;
using HelpDesk.Domain.Entities.Users;
using HelpDesk.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HelpDesk.Application.Tests.Tickets.Commands.AssignTicket;

public sealed class AssignTicketCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnTicketNotFound_WhenTicketDoesNotExist()
    {
        // Arrange
        await using var context = CreateContext();

        var currentUserService = CreateCurrentUserService(1);

        var handler = new AssignTicketCommandHandler(
            context,
            currentUserService.Object);

        var command = new AssignTicketCommand(
            TicketId: 999,
            UserId: 2);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TicketErrors.TicketNotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnAgentNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        await using var context = CreateContext();

        var ticket = Ticket.Create(
            "Test Ticket",
            "Test Description",
            categoryId: 1,
            TicketPriority.Medium,
            createdByUserId: 1);

        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();

        var currentUserService = CreateCurrentUserService(1);

        var handler = new AssignTicketCommandHandler(
            context,
            currentUserService.Object);

        var command = new AssignTicketCommand(
            TicketId: ticket.Id,
            UserId: 999);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(AgentErrors.AgentNotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnUserIsNotAnAgent_WhenUserIsNotAgent()
    {
        // Arrange
        await using var context = CreateContext();

        var ticket = Ticket.Create(
            "Test Ticket",
            "Test Description",
            categoryId: 1,
            TicketPriority.Medium,
            createdByUserId: 1);

        var user = User.Create(
            "Test Customer",
            "customer@test.com",
            "hashed-password",
            UserRole.Customer);

        context.Tickets.Add(ticket);
        context.Users.Add(user);

        await context.SaveChangesAsync();

        var currentUserService = CreateCurrentUserService(1);

        var handler = new AssignTicketCommandHandler(
            context,
            currentUserService.Object);

        var command = new AssignTicketCommand(
            TicketId: ticket.Id,
            UserId: user.Id);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TicketErrors.UserIsNotAnAgent);
    }

    [Fact]
    public async Task Handle_ShouldAssignTicket_WhenUserIsAgent()
    {
        // Arrange
        await using var context = CreateContext();

        var ticket = Ticket.Create(
            "Test Ticket",
            "Test Description",
            categoryId: 1,
            TicketPriority.High,
            createdByUserId: 1);

        var agent = User.Create(
            "Test Agent",
            "agent@test.com",
            "hashed-password",
            UserRole.Agent);

        context.Tickets.Add(ticket);
        context.Users.Add(agent);

        await context.SaveChangesAsync();

        var currentUserId = 10;

        var currentUserService =
            CreateCurrentUserService(currentUserId);

        var handler = new AssignTicketCommandHandler(
            context,
            currentUserService.Object);

        var command = new AssignTicketCommand(
            TicketId: ticket.Id,
            UserId: agent.Id);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var assignedTicket = await context.Tickets
            .SingleAsync(x => x.Id == ticket.Id);

        assignedTicket.AssigneeId.Should().Be(agent.Id);
        assignedTicket.AssigneeId.Should().Be(agent.Id);
    }

    private static TestApplicationDbContext CreateContext()
    {
        var options =
            new DbContextOptionsBuilder<TestApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        return new TestApplicationDbContext(options);
    }

    private static Mock<ICurrentUserService> CreateCurrentUserService(
        int userId)
    {
        var mock = new Mock<ICurrentUserService>();

        mock.Setup(x => x.UserId)
            .Returns(userId);

        return mock;
    }
}