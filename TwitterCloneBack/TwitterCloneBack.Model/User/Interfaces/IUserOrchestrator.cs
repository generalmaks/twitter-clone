using TwitterCloneBack.Model.User.Contracts;
using TwitterCloneBack.Model.User.Model;

namespace TwitterCloneBack.Model.User.Interfaces;

public interface IUserOrchestrator
{
    Task<UserDto> GetUserByIdAsync(int id);
    Task<List<UserDto>> GetUsersAsync(int page, int pageSize);
    Task<UserDto> CreateUserAsync(CreateUser userDto);
    Task<UserDto> UpdateUserAsync(UpdateUser userDto);
    Task<UserDto> DeleteUserAsync(int id);
}
