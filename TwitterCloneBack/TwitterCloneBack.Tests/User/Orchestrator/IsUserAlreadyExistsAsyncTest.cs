using Moq;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.Model.User.Model;
using TwitterCloneBack.Orchestrator.User.Orchestrator;

namespace TwitterCloneBack.Tests.User.Orchestrator;

public class IsUserAlreadyExistsAsyncTest
{
    private readonly Mock<IUserRepository> _repositoryMock = new();

    private UserOrchestrator CreateUserOrchestrator()
    {
        return new UserOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task IsUserAlreadyExistsAsync_IfUserExist_ReturnsTrue()
    {
        // Arrange
        var user = new UserDto();
        
        _repositoryMock
            .Setup(r => r.IsUserAlreadyExistsAsync(user.Email, user.Username))
            .ReturnsAsync(true);

        var userOrchestrator = CreateUserOrchestrator();
        
        // Act & Assert
        Assert.True(await userOrchestrator.IsUserAlreadyExistsAsync(user.Email, user.Username));
    }
    
    [Fact]
    public async Task IsUserAlreadyExistsAsync_IfDoesntExist_ReturnsFalse()
    {
        // Arrange
        var user = new UserDto();
        
        _repositoryMock
            .Setup(r => r.IsUserAlreadyExistsAsync(user.Email, user.Username))
            .ReturnsAsync(false);

        var userOrchestrator = CreateUserOrchestrator();
        
        // Act & Assert
        Assert.False(await userOrchestrator.IsUserAlreadyExistsAsync(user.Email, user.Username));
    }
}