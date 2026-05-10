using Moq;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.Model.User.Model;
using TwitterCloneBack.Orchestrator.User.Orchestrator;

namespace TwitterCloneBack.Tests.User.Orchestrator;

public class GetByIdAsyncTests()
{
    private readonly Mock<IUserRepository> _repositoryMock = new();

    private UserOrchestrator CreateUserOrchestrator()
    {
        return new UserOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenUserExists_ReturnsUser()
    {
        // Arrange
        int id = 1;
        var user =
            new UserDto
            {
                Id = id
            };

        _repositoryMock
            .Setup(r => r.GetUserByIdAsync(id))
            .ReturnsAsync(user);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        var result = await userOrchestrator.GetUserByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(user, result);
    }

    [Fact]
    public async Task GetByIdAsync_IfUserDoesntExist_ThrowsError()
    {
        // Arrange
        int id = 1;

        _repositoryMock
            .Setup(r => r.GetUserByIdAsync(id))
            .ReturnsAsync((UserDto)null!);

        var userOrchestrator = CreateUserOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await userOrchestrator.GetUserByIdAsync(id));
    }
}