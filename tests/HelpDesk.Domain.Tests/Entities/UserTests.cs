using FluentAssertions;
using HelpDesk.Domain.Entities.Users;

namespace HelpDesk.Domain.Tests.Entities;

public sealed class UserTests
{
    [Fact]
    public void Create_ShouldCreateUser_WhenValidDataIsProvided()
    {
        // Arrange
        const string fullName = "John Doe";
        const string email = "john@example.com";
        const string passwordHash = "hashed-password";

        // Act
        var user = User.Create(
            fullName,
            email,
            passwordHash);

        // Assert
        user.FullName.Should().Be(fullName);
        user.Email.Should().Be(email);
        user.PasswordHash.Should().Be(passwordHash);
        user.Role.Should().Be(UserRole.Customer);
    }

    [Fact]
    public void Create_ShouldUseProvidedRole()
    {
        // Act
        var user = User.Create(
            "John Doe",
            "john@example.com",
            "hashed-password",
            UserRole.Agent);

        // Assert
        user.Role.Should().Be(UserRole.Agent);
    }

    [Fact]
    public void Create_ShouldTrimFullName()
    {
        // Act
        var user = User.Create(
            "  John Doe  ",
            "john@example.com",
            "hashed-password");

        // Assert
        user.FullName.Should().Be("John Doe");
    }

    [Fact]
    public void Create_ShouldNormalizeEmail()
    {
        // Act
        var user = User.Create(
            "John Doe",
            "  JOHN@EXAMPLE.COM  ",
            "hashed-password");

        // Assert
        user.Email.Should().Be("john@example.com");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_ShouldThrow_WhenFullNameIsInvalid(
        string? fullName)
    {
        // Act
        var act = () => User.Create(
            fullName!,
            "john@example.com",
            "hashed-password");

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("fullName");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_ShouldThrow_WhenEmailIsInvalid(
        string? email)
    {
        // Act
        var act = () => User.Create(
            "John Doe",
            email!,
            "hashed-password");

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("email");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_ShouldThrow_WhenPasswordHashIsInvalid(
        string? passwordHash)
    {
        // Act
        var act = () => User.Create(
            "John Doe",
            "john@example.com",
            passwordHash!);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("passwordHash");
    }

    [Fact]
    public void Create_ShouldThrow_WhenRoleIsInvalid()
    {
        // Arrange
        var invalidRole = (UserRole)999;

        // Act
        var act = () => User.Create(
            "John Doe",
            "john@example.com",
            "hashed-password",
            invalidRole);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("role");
    }

    [Fact]
    public void ChangeFullName_ShouldUpdateFullName()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.ChangeFullName("Jane Doe");

        // Assert
        user.FullName.Should().Be("Jane Doe");
    }

    [Fact]
    public void ChangeFullName_ShouldTrimFullName()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.ChangeFullName("  Jane Doe  ");

        // Assert
        user.FullName.Should().Be("Jane Doe");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void ChangeFullName_ShouldThrow_WhenFullNameIsInvalid(
        string? fullName)
    {
        // Arrange
        var user = CreateUser();

        // Act
        var act = () => user.ChangeFullName(fullName!);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("fullName");
    }

    [Fact]
    public void ChangeEmail_ShouldUpdateEmail()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.ChangeEmail("new@example.com");

        // Assert
        user.Email.Should().Be("new@example.com");
    }

    [Fact]
    public void ChangeEmail_ShouldNormalizeEmail()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.ChangeEmail("  NEW@EXAMPLE.COM  ");

        // Assert
        user.Email.Should().Be("new@example.com");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void ChangeEmail_ShouldThrow_WhenEmailIsInvalid(
        string? email)
    {
        // Arrange
        var user = CreateUser();

        // Act
        var act = () => user.ChangeEmail(email!);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("email");
    }

    [Fact]
    public void ChangePasswordHash_ShouldUpdatePasswordHash()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.ChangePasswordHash("new-hashed-password");

        // Assert
        user.PasswordHash.Should().Be("new-hashed-password");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void ChangePasswordHash_ShouldThrow_WhenPasswordHashIsInvalid(
        string? passwordHash)
    {
        // Arrange
        var user = CreateUser();

        // Act
        var act = () => user.ChangePasswordHash(passwordHash!);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("passwordHash");
    }

    [Theory]
    [InlineData(UserRole.Customer)]
    [InlineData(UserRole.Agent)]
    [InlineData(UserRole.Admin)]
    public void ChangeRole_ShouldUpdateRole_WhenRoleIsValid(
        UserRole role)
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.ChangeRole(role);

        // Assert
        user.Role.Should().Be(role);
    }

    [Fact]
    public void ChangeRole_ShouldThrow_WhenRoleIsInvalid()
    {
        // Arrange
        var user = CreateUser();
        var invalidRole = (UserRole)999;

        // Act
        var act = () => user.ChangeRole(invalidRole);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithParameterName("role");
    }

    [Fact]
    public void IsAgent_ShouldReturnTrue_WhenUserIsAgent()
    {
        // Arrange
        var user = User.Create(
            "John Doe",
            "john@example.com",
            "hashed-password",
            UserRole.Agent);

        // Act
        var result = user.IsAgent();

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(UserRole.Customer)]
    [InlineData(UserRole.Admin)]
    public void IsAgent_ShouldReturnFalse_WhenUserIsNotAgent(
        UserRole role)
    {
        // Arrange
        var user = User.Create(
            "John Doe",
            "john@example.com",
            "hashed-password",
            role);

        // Act
        var result = user.IsAgent();

        // Assert
        result.Should().BeFalse();
    }

    private static User CreateUser()
    {
        return User.Create(
            "John Doe",
            "john@example.com",
            "hashed-password");
    }
}
