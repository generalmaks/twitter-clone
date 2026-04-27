using TwitterCloneBack.Model.User.Model;

namespace TwitterCloneBack.Model.User.Interfaces;

public interface IUserRepository
{
    Task<UserDto> GetUserByIdAsync(int id);
    Task<List<UserDto>> GetUsersAsync(int page, int pageSize);
    Task<UserDto> CreateUserAsync(UserDto userDto);
    Task<UserDto> UpdateUserAsync(UserDto userDto);
    Task<UserDto> DeleteUserAsync(int id);
}
