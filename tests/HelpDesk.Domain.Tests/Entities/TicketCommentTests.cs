//using FluentAssertions;
//using HelpDesk.Domain.Entities.Tickets;

//namespace HelpDesk.Domain.Tests.Entities;

//public sealed class TicketCommentTests
//{
//    [Fact]
//    public void Create_ShouldCreateComment_WithValidContent()
//    {
//        // Act
//        var comment = TicketComment.Create("This is a comment.");

//        // Assert
//        comment.Content.Should().Be("This is a comment.");
//        comment.CreatedAtUtc.Should().BeCloseTo(
//            DateTime.UtcNow,
//            TimeSpan.FromSeconds(1));
//    }

//    [Fact]
//    public void Create_ShouldTrimContent()
//    {
//        // Act
//        var comment = TicketComment.Create("  This is a comment.  ");

//        // Assert
//        comment.Content.Should().Be("This is a comment.");
//    }

//    [Theory]
//    [InlineData("")]
//    [InlineData(" ")]
//    [InlineData("   ")]
//    public void Create_ShouldThrow_WhenContentIsEmpty(
//        string content)
//    {
//        // Act
//        var act = () => TicketComment.Create(content);

//        // Assert
//        act.Should()
//            .Throw<ArgumentException>()
//            .WithParameterName("content");
//    }

//    [Fact]
//    public void UpdateContent_ShouldUpdateContent()
//    {
//        // Arrange
//        var comment = TicketComment.Create("Original content.");

//        // Act
//        comment.UpdateContent("Updated content.");

//        // Assert
//        comment.Content.Should().Be("Updated content.");
//    }

//    [Fact]
//    public void UpdateContent_ShouldTrimContent()
//    {
//        // Arrange
//        var comment = TicketComment.Create("Original content.");

//        // Act
//        comment.UpdateContent("  Updated content.  ");

//        // Assert
//        comment.Content.Should().Be("Updated content.");
//    }

//    [Theory]
//    [InlineData("")]
//    [InlineData(" ")]
//    [InlineData("   ")]
//    public void UpdateContent_ShouldThrow_WhenContentIsEmpty(
//        string content)
//    {
//        // Arrange
//        var comment = TicketComment.Create("Original content.");

//        // Act
//        var act = () => comment.UpdateContent(content);

//        // Assert
//        act.Should()
//            .Throw<ArgumentException>()
//            .WithParameterName("content");
//    }
//}