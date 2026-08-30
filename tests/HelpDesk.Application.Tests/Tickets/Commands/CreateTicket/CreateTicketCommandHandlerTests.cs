using FluentAssertions;
using HelpDesk.Application.Common.Interfaces;
using HelpDesk.Application.Errors;
using HelpDesk.Application.Tests.Common;
using HelpDesk.Application.Tickets.Commands.CreateTicket;
using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace HelpDesk.Application.Tests.Tickets.Commands.CreateTicket;

public sealed class CreateTicketCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnCategoryNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        await using var context = CreateContext();

        var currentUserService = CreateCurrentUserService(1);
        var logger = CreateLogger();

        var handler = new CreateTicketCommandHandler(
            context,
            currentUserService.Object,
            logger.Object);

        var command = new CreateTicketCommand(
            "Test Ticket",
            "Test Description",
            999,
            TicketPriority.Medium);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(TicketErrors.CategoryNotFound);

        context.Tickets.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldCreateTicket_WhenCategoryExists()
    {
        // Arrange
        await using var context = CreateContext();

        var category = Category.Create("Technical Support");

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var currentUserService = CreateCurrentUserService(10);
        var logger = CreateLogger();

        var handler = new CreateTicketCommandHandler(
            context,
            currentUserService.Object,
            logger.Object);

        var command = new CreateTicketCommand(
            "Test Ticket",
            "Test Description",
            category.Id,
            TicketPriority.High);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var ticket = await context.Tickets
            .SingleAsync();

        ticket.Title.Should().Be("Test Ticket");
        ticket.Description.Should().Be("Test Description");
        ticket.CategoryId.Should().Be(category.Id);
        ticket.Priority.Should().Be(TicketPriority.High);
        ticket.Status.Should().Be(TicketStatus.Open);
        ticket.CreatedByUserId.Should().Be(10);
    }

    [Fact]
    public async Task Handle_ShouldUseCurrentUserId_WhenCreatingTicket()
    {
        // Arrange
        await using var context = CreateContext();

        var category = Category.Create("Technical Support");

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        const int currentUserId = 42;

        var currentUserService =
            CreateCurrentUserService(currentUserId);

        var logger = CreateLogger();

        var handler = new CreateTicketCommandHandler(
            context,
            currentUserService.Object,
            logger.Object);

        var command = new CreateTicketCommand(
            "Test Ticket",
            "Test Description",
            category.Id,
            TicketPriority.Low);

        // Act
        await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        var ticket = await context.Tickets
            .SingleAsync();

        ticket.CreatedByUserId.Should().Be(currentUserId);
    }

    [Fact]
    public async Task Handle_ShouldReturnCreatedTicketResponse_WhenSuccessful()
    {
        // Arrange
        await using var context = CreateContext();

        var category = Category.Create("Technical Support");

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var currentUserService = CreateCurrentUserService(10);
        var logger = CreateLogger();

        var handler = new CreateTicketCommandHandler(
            context,
            currentUserService.Object,
            logger.Object);

        var command = new CreateTicketCommand(
            "Test Ticket",
            "Test Description",
            category.Id,
            TicketPriority.Critical);

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeNull();
        result.Value.Id.Should().BeGreaterThan(0);
        result.Value.Title.Should().Be("Test Ticket");
        result.Value.Status.Should().Be(TicketStatus.Open);
        result.Value.CreatedAtUtc.Should().NotBe(default);
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

    private static Mock<ILogger<CreateTicketCommandHandler>> CreateLogger()
    {
        return new Mock<ILogger<CreateTicketCommandHandler>>();
    }
}
