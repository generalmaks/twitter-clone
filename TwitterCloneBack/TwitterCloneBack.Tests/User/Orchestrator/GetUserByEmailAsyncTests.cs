using Moq;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.Model.User.Model;
using TwitterCloneBack.Orchestrator.User.Orchestrator;

namespace TwitterCloneBack.Tests.User.Orchestrator;

public class GetUserByEmailAsyncTests
{
    private readonly Mock<IUserRepository> _repositoryMock = new();

    private UserOrchestrator CreateUserOrchestrator()
    {
        return new UserOrchestrator(_repositoryMock.Object);
    }
    
    [Fact]
    public async Task GetUserByEmailAsync_WhenUserExists_ReturnsUser()
    {
        // Arrange
        string email = "test@test.com";
        var user =
            new UserDto
            {
                Email = email
            };

        _repositoryMock
            .Setup(r => r.GetUserByEmailAsync(email))
            .ReturnsAsync(user);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        var result = await userOrchestrator.GetUserByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(user, result);
    }

    [Fact]
    public async Task GetByIdAsync_IfUserDoesntExist_ThrowsError()
    {
        // Arrange
        string email = "test@test.com";

        _repositoryMock
            .Setup(r => r.GetUserByEmailAsync(email))
            .ReturnsAsync((UserDto)null!);

        var userOrchestrator = CreateUserOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await userOrchestrator.GetUserByEmailAsync(email));
    }
}