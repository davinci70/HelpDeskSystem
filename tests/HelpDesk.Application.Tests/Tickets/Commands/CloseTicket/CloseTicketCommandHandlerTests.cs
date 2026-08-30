using FluentAssertions;
using HelpDesk.Application.Common.Interfaces;
using HelpDesk.Application.Errors;
using HelpDesk.Application.Tests.Common;
using HelpDesk.Application.Tickets.Commands.CloseTicket;
using HelpDesk.Domain.Entities.Tickets;
using HelpDesk.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HelpDesk.Application.Tests.Tickets.Commands.CloseTicket;

public sealed class CloseTicketCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnTicketNotFound_WhenTicketDoesNotExist()
    {
        // Arrange
        await using var context = CreateContext();

        var currentUser = CreateCurrentUserService(1);

        var handler = new CloseTicketCommandHandler(
            context,
            currentUser.Object);

        var command = new CloseTicketCommand(
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
    public async Task Handle_ShouldReturnCannotClose_WhenTicketIsNotResolved()
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

        var handler = new CloseTicketCommandHandler(
            context,
            currentUser.Object);

        var command = new CloseTicketCommand(
            TicketId: ticket.Id);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TicketErrors.CannotClose);

        ticket.Status.Should().Be(TicketStatus.Open);
    }

    [Fact]
    public async Task Handle_ShouldCloseTicket_WhenTicketIsResolved()
    {
        // Arrange
        await using var context = CreateContext();

        var ticket = Ticket.Create(
            "Test Ticket",
            "Test Description",
            categoryId: 1,
            TicketPriority.High,
            createdByUserId: 1);

        ticket.Assign(
            userId: 2,
            actionByUserId: 1);

        ticket.StartProgress(
            actionByUserId: 1);

        ticket.Resolve(
            actionByUserId: 1);

        context.Tickets.Add(ticket);

        await context.SaveChangesAsync();

        var currentUser = CreateCurrentUserService(1);

        var handler = new CloseTicketCommandHandler(
            context,
            currentUser.Object);

        var command = new CloseTicketCommand(
            TicketId: ticket.Id);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        ticket.Status.Should().Be(TicketStatus.Closed);
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