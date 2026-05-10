using Moq;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.Model.User.Model;
using TwitterCloneBack.Orchestrator.User.Orchestrator;

namespace TwitterCloneBack.Tests.User.Orchestrator;

public class DeleteUserAsyncTests
{
    private readonly Mock<IUserRepository> _repositoryMock = new();

    private UserOrchestrator CreateUserOrchestrator()
    {
        return new UserOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserDoesntExist_ThrowsException()
    {
        // Arrange
        int id = 1;

        _repositoryMock
            .Setup(r => r.DeleteUserAsync(id))
            .ReturnsAsync((UserDto)null!);
        
        var userOrchestrator = CreateUserOrchestrator();
        
        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await userOrchestrator.DeleteUserAsync(id));
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserExist_DeleteUser()
    {
        // Arrange
        int id = 1;
        var user = new UserDto
        {
            Id = id,
            Username = "test",
            Email = "test@test.com",
            DisplayUsername = "Test User"
        };

        _repositoryMock
            .Setup(r => r.DeleteUserAsync(id))
            .ReturnsAsync(user);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        var result = await userOrchestrator.DeleteUserAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(user, result);
    }

    [Fact]
    public async Task DeleteUserAsync_ForwardsIdToRepository()
    {
        // Arrange
        int id = 1;
        var user = new UserDto
        {
            Id = id
        };

        _repositoryMock
            .Setup(r => r.DeleteUserAsync(id))
            .ReturnsAsync(user);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        await userOrchestrator.DeleteUserAsync(id);

        // Assert
        _repositoryMock.Verify(
            r => r.DeleteUserAsync(id),
            Times.Once);
    }
}
