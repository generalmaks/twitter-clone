using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBack.Dal.Like.Dao;
using TwitterCloneBack.Model.Like.Interfaces;
using TwitterCloneBack.Model.Like.Model;

namespace TwitterCloneBack.Dal.Like.Repository;

public class LikeRepository(
    TwitterCloneContext db,
    IMapper mapper) : ILikeRepository
{
    public async Task<LikeDto> GetLikeByIdAsync(int likeId)
    {
        return mapper.Map<LikeDto>(
            await db.Likes
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == likeId));
    }

    public async Task<LikeDto> CreateLikeAsync(int postId, int userId)
    {
        var like =
            await db.Likes.AddAsync(new LikeDao
            {
                PostId = postId,
                LikedById = userId
            });
        await db.SaveChangesAsync();
        return mapper.Map<LikeDto>(like.Entity);
    }

    public async Task<LikeDto> RemoveLikeAsync(int postId, int userId)
    {
        var like =
            await db.Likes.FirstOrDefaultAsync(l =>
                l.PostId == postId && l.LikedById == userId);
        if (like is null)
            return null!;

        db.Likes.Remove(like);
        await db.SaveChangesAsync();
        return mapper.Map<LikeDto>(like);
    }

    public async Task<int> CountByPostIdAsync(int postId)
    {
        return await db.Likes.CountAsync(l => l.PostId == postId);
    }

    public async Task<bool> IsPostLikedByUserAsync(int postId, int userId)
    {
        return await db.Likes.AnyAsync(l =>
            l.PostId == postId && l.LikedById == userId);
    }

    public async Task<List<LikeDto>> GetLikesOnPostAsync(int postId)
    {
        var likes =
            await db.Likes
                .AsNoTracking()
                .Where(l => l.PostId == postId)
                .ToListAsync();
        return mapper.Map<List<LikeDto>>(likes);
    }

    public async Task<List<LikeDto>> GetAllLikesFromUserAsync(int userId)
    {
        var likes =
            await db.Likes
                .AsNoTracking()
                .Where(l => l.LikedById == userId)
                .ToListAsync();
        return mapper.Map<List<LikeDto>>(likes);
    }
}