using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBack.Dal.User.Dao;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.Model.User.Model;

namespace TwitterCloneBack.Dal.User.Repository;

public class UserRepository(TwitterCloneContext db, IMapper mapper) : IUserRepository
{
    public async Task<UserDto> GetUserByIdAsync(int id) =>
        mapper.Map<UserDto>(await db.Users.FirstOrDefaultAsync(u => u.Id == id));

    public async Task<List<UserDto>> GetUsersAsync(int page, int pageSize) =>
        mapper.Map<List<UserDto>>(
            await db.Users
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
        );

    public async Task<UserDto> CreateUserAsync(UserDto userDto)
    {
        var createdUserEntity = await db.Users.AddAsync(mapper.Map<UserDao>(userDto));
        await db.SaveChangesAsync();
        return mapper.Map<UserDto>(createdUserEntity.Entity);
    }

    public async Task<UserDto> UpdateUserAsync(UserDto userDto)
    {
        var existingUser = await db.Users.FirstOrDefaultAsync(u => u.Id == userDto.Id);
        if (existingUser is null)
        {
            return null!;
        }

        mapper.Map(userDto, existingUser);
        await db.SaveChangesAsync();
        return mapper.Map<UserDto>(existingUser);
    }

    public async Task<UserDto> DeleteUserAsync(int id)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            return null!;
        }

        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return mapper.Map<UserDto>(user);
    }
}
