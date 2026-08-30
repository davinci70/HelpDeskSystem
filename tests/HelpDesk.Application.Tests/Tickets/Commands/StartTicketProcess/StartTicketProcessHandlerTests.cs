using FluentAssertions;
using HelpDesk.Application.Abstractions.Persistence;
using HelpDesk.Application.Common.Interfaces;
using HelpDesk.Application.Errors;
using HelpDesk.Application.Tests.Common;
using HelpDesk.Application.Tickets.Commands.StartTicketProcess;
using HelpDesk.Domain.Entities.Tickets;
using HelpDesk.Domain.Entities.Users;
using HelpDesk.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HelpDesk.Application.Tests.Tickets.Commands.StartTicketProcess;

public sealed class StartTicketProcessHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnTicketNotFound_WhenTicketDoesNotExist()
    {
        // Arrange
        await using var context = CreateContext();

        var currentUser = CreateCurrentUserService(1);

        var handler = new StartTicketProcessHandler(
            context,
            currentUser.Object);

        var command = new StartTicketProcessCommand(
            TicketId: 999);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TicketErrors.TicketNotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotAssigned_WhenTicketHasNoAssignee()
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

        var currentUser = CreateCurrentUserService(1);

        var handler = new StartTicketProcessHandler(
            context,
            currentUser.Object);

        var command = new StartTicketProcessCommand(
            TicketId: ticket.Id);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TicketErrors.NotAssigned);

        ticket.Status.Should().Be(TicketStatus.Open);
    }

    [Fact]
    public async Task Handle_ShouldReturnCannotStart_WhenTicketIsNotOpen()
    {
        // Arrange
        await using var context = CreateContext();

        var ticket = Ticket.Create(
            "Test Ticket",
            "Test Description",
            categoryId: 1,
            TicketPriority.Medium,
            createdByUserId: 1);

        ticket.Assign(
            userId: 2,
            actionByUserId: 1);

        ticket.StartProgress(
            actionByUserId: 1);

        context.Tickets.Add(ticket);

        await context.SaveChangesAsync();

        var currentUser = CreateCurrentUserService(1);

        var handler = new StartTicketProcessHandler(
            context,
            currentUser.Object);

        var command = new StartTicketProcessCommand(
            TicketId: ticket.Id);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TicketErrors.CannotStart);

        ticket.Status.Should().Be(TicketStatus.InProgress);
    }

    [Fact]
    public async Task Handle_ShouldStartTicket_WhenTicketIsOpenAndAssigned()
    {
        // Arrange
        await using var context = CreateContext();

        var ticket = Ticket.Create(
            "Test Ticket",
            "Test Description",
            categoryId: 1,
            TicketPriority.Medium,
            createdByUserId: 1);

        ticket.Assign(
            userId: 2,
            actionByUserId: 1);

        context.Tickets.Add(ticket);

        await context.SaveChangesAsync();

        var currentUser = CreateCurrentUserService(1);

        var handler = new StartTicketProcessHandler(
            context,
            currentUser.Object);

        var command = new StartTicketProcessCommand(
            TicketId: ticket.Id);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        ticket.Status.Should().Be(TicketStatus.InProgress);
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