using TwitterCloneBack.Model.Contracts;
using TwitterCloneBack.Model.Model;

namespace TwitterCloneBack.Model.Interfaces;

public interface IUserOrchestrator
{
    Task<UserDto> GetUserByIdAsync(int id);
    Task<List<UserDto>> GetUsersAsync(int page, int pageSize);
    Task<UserDto> CreateUserAsync(CreateUser userDto);
    Task<UserDto> UpdateUserAsync(UpdateUser userDto);
    Task<UserDto> DeleteUserAsync(int id);
}
