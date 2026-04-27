using System.Security.Cryptography;
using System.Text;
using TwitterCloneBack.Model.User.Contracts;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.Model.User.Model;

namespace TwitterCloneBack.Orchestrator.User.Orchestrator;

public class UserOrchestrator(IUserRepository userRepository) : IUserOrchestrator
{
    public async Task<UserDto> GetUserByIdAsync(int id) =>
        await userRepository.GetUserByIdAsync(id) ?? throw new KeyNotFoundException($"User with id {id} not found");

    public async Task<List<UserDto>> GetUsersAsync(int page, int pageSize) =>
        await userRepository.GetUsersAsync(page, pageSize);

    public async Task<UserDto> CreateUserAsync(CreateUser userDto) =>
        await userRepository.CreateUserAsync(new UserDto
        {
            Username = userDto.Username,
            Email = userDto.Email,
            DisplayUsername = userDto.DisplayUsername,
            Bio = userDto.Bio,
            PasswordHash = HashPassword(userDto.Password)
        });

    public async Task<UserDto> UpdateUserAsync(UpdateUser updateUser)
    {
        var user = await userRepository.GetUserByIdAsync(updateUser.Id)
                   ?? throw new KeyNotFoundException($"User with id {updateUser.Id} not found");

        user.Username = updateUser.Username ?? user.Username;
        user.DisplayUsername = updateUser.DisplayUsername ?? user.DisplayUsername;
        user.Bio = updateUser.Bio ?? user.Bio;
        if (updateUser.Password != null)
        {
            if (updateUser.Password.Length < 8)
                throw new ArgumentException("Password length cant be shorter than 8 characters");

            user.PasswordHash = HashPassword(updateUser.Password) ?? user.PasswordHash;
        }

        return await userRepository.UpdateUserAsync(user);
    }

    public async Task<UserDto> DeleteUserAsync(int id) =>
        await userRepository.DeleteUserAsync(id) ?? throw new KeyNotFoundException($"User with id {id} not found");

    private static byte[] HashPassword(string password)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password) ?? throw new ArgumentException("Could not hash password.");
        return Encoding.UTF8.GetBytes(hash);
    }
}