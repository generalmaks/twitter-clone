using Moq;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.Model.User.Model;
using TwitterCloneBack.Orchestrator.User.Orchestrator;

namespace TwitterCloneBack.Tests.User.Orchestrator;

public class GetUsersAsyncTests
{
    private readonly Mock<IUserRepository> _repositoryMock = new();

    private UserOrchestrator CreateUserOrchestrator()
    {
        return new UserOrchestrator(_repositoryMock.Object);
    }

    [Fact]
    public async Task
        GetUsersAsync_WhenPageCountEqualsUserAmount_ReturnsAllUsers()
    {
        // Arrange
        int page = 1;
        int pageSize = 2;
        var users =
            new List<UserDto>
            {
                new()
                {
                    Id = 1,
                    Username = "test1",
                    Email = "test1@test.com"
                },
                new()
                {
                    Id = 2,
                    Username = "test2",
                    Email = "test2@test.com"
                }
            };

        _repositoryMock
            .Setup(r => r.GetUsersAsync(page, pageSize))
            .ReturnsAsync(users);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        var result = await userOrchestrator.GetUsersAsync(page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(users, result);
    }

    [Fact]
    public async Task GetUsersAsync_WhenNoUsersExist_ReturnsEmptyList()
    {
        // Arrange
        int page = 1;
        int pageSize = 20;
        var users = new List<UserDto>();

        _repositoryMock
            .Setup(r => r.GetUsersAsync(page, pageSize))
            .ReturnsAsync(users);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        var result = await userOrchestrator.GetUsersAsync(page, pageSize);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task
        GetUsersAsync_WhenPageSizeLessThanUsersAmount_ReturnsPagesizeAmountOfUsers()
    {
        // Arrange
        int page = 1;
        int pageSize = 1;
        var users =
            new List<UserDto>
            {
                new()
                {
                    Id = 1,
                    Username = "test1",
                    Email = "test1@test.com"
                },
                new()
                {
                    Id = 2,
                    Username = "test2",
                    Email = "test2@test.com"
                }
            };

        _repositoryMock
            .Setup(r => r.GetUsersAsync(page, pageSize))
            .ReturnsAsync(users.Slice(1, 1));

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        var result = await userOrchestrator.GetUsersAsync(page, pageSize);

        // Assert
        Assert.Equivalent(result.Count, pageSize);
    }

    [Fact]
    public async Task GetUsersAsync_ForwardsPaginationToRepository()
    {
        // Arrange
        int page = 3;
        int pageSize = 10;

        _repositoryMock
            .Setup(r => r.GetUsersAsync(page, pageSize))
            .ReturnsAsync([]);

        var userOrchestrator = CreateUserOrchestrator();

        // Act
        await userOrchestrator.GetUsersAsync(page, pageSize);

        // Assert
        _repositoryMock.Verify(
            r => r.GetUsersAsync(page, pageSize),
            Times.Once);
    }

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 0)]
    [InlineData(-1, 20)]
    [InlineData(1, -5)]
    [InlineData(0, 0)]
    [InlineData(-1, -1)]
    public async Task GetUsersAsync_InvalidArguments_ThrowsArgumentsException(
        int page,
        int pageSize
    )
    {
        // Arrange
        var userOrchestrator = CreateUserOrchestrator();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await userOrchestrator.GetUsersAsync(page, pageSize));
    }
}