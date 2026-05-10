using System.Text;
using Moq;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.Model.User.Model;
using TwitterCloneBack.Orchestrator.User.Orchestrator;

namespace TwitterCloneBack.Tests.User.Orchestrator;

public class IsCorrectPasswordTests
{
    private readonly Mock<IUserRepository> _repositoryMock = new();

    private UserOrchestrator CreateUserOrchestrator()
    {
        return new UserOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task IsCorrectPassword_WhenPasswordIsCorrect_ReturnsTrue()
    {
        // Arrange
        int userId = 1;
        string password = "12345678";
        var user = CreateUserWithPassword(userId, password);

        _repositoryMock
            .Setup(r => r.GetUserByIdAsync(userId))
            .ReturnsAsync(user);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        var result = await userOrchestrator.IsCorrectPassword(userId, password);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsCorrectPassword_WhenPasswordIsIncorrect_ReturnsFalse()
    {
        // Arrange
        int userId = 1;
        string correctPassword = "12345678";
        string incorrectPassword = "87654321";
        var user = CreateUserWithPassword(userId, correctPassword);

        _repositoryMock
            .Setup(r => r.GetUserByIdAsync(userId))
            .ReturnsAsync(user);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        var result =
            await userOrchestrator.IsCorrectPassword(
                userId,
                incorrectPassword);

        // Assert
        Assert.False(result);
    }

    private static UserDto CreateUserWithPassword(int id, string password)
    {
        return new UserDto
        {
            Id = id,
            PasswordHash =
                Encoding.UTF8.GetBytes(BCrypt.Net.BCrypt.HashPassword(password))
        };
    }
}
