using HelpDesk.Domain.Entities;

namespace HelpDesk.Domain.Tests.Entities;

public sealed class CategoryTests
{
    [Fact]
    public void Create_WithValidName_ShouldCreateCategory()
    {
        // Arrange
        const string name = "Technical Support";

        // Act
        var category = Category.Create(name);

        // Assert
        Assert.Equal(name, category.Name);
    }

    [Fact]
    public void Create_WithNameContainingWhitespace_ShouldTrimName()
    {
        // Arrange
        const string name = "  Technical Support  ";

        // Act
        var category = Category.Create(name);

        // Assert
        Assert.Equal("Technical Support", category.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_WithEmptyName_ShouldThrowArgumentException(string name)
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => Category.Create(name));

        // Assert
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Create_WithNullName_ShouldThrowArgumentException()
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => Category.Create(null!));

        // Assert
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Rename_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var category = Category.Create("Technical Support");

        // Act
        category.Rename("Customer Support");

        // Assert
        Assert.Equal("Customer Support", category.Name);
    }

    [Fact]
    public void Rename_WithNameContainingWhitespace_ShouldTrimName()
    {
        // Arrange
        var category = Category.Create("Technical Support");

        // Act
        category.Rename("  Customer Support  ");

        // Assert
        Assert.Equal("Customer Support", category.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Rename_WithEmptyName_ShouldThrowArgumentException(string name)
    {
        // Arrange
        var category = Category.Create("Technical Support");

        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => category.Rename(name));

        // Assert
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Rename_WithNullName_ShouldThrowArgumentException()
    {
        // Arrange
        var category = Category.Create("Technical Support");

        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => category.Rename(null!));

        // Assert
        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Rename_WithInvalidName_ShouldKeepOriginalName()
    {
        // Arrange
        var category = Category.Create("Technical Support");

        // Act
        Assert.Throws<ArgumentException>(
            () => category.Rename("   "));

        // Assert
        Assert.Equal("Technical Support", category.Name);
    }
}
