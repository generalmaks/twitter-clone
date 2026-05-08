using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBack.Dal.Post.Dao;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.Post.Model;

namespace TwitterCloneBack.Dal.Post.Repository;

public class PostRepository(
    TwitterCloneContext db,
    IMapper mapper) : IPostRepository
{
    public async Task<PostDto> GetPostByIdAsync(int id)
    {
        return mapper.Map<PostDto>(
            await db.Posts
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted));
    }

    public async Task<List<PostDto>> GetPostsAsync(int page, int pageSize)
    {
        return mapper.Map<List<PostDto>>(
            await db.Posts
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync());
    }

    public async Task<PostDto> CreatePostAsync(PostDto postDto)
    {
        var postEntity = mapper.Map<PostDao>(postDto);
        var createdPost = await db.Posts.AddAsync(postEntity);
        await db.SaveChangesAsync();
        return mapper.Map<PostDto>(createdPost.Entity);
    }

    public async Task<PostDto> UpdatePostAsync(PostDto postDto)
    {
        var existingPost =
            await db.Posts.FirstOrDefaultAsync(p => p.Id == postDto.Id);
        if (existingPost is null) return null!;

        mapper.Map(postDto, existingPost);
        await db.SaveChangesAsync();
        return mapper.Map<PostDto>(existingPost);
    }

    public async Task<PostDto> DeletePostAsync(int id)
    {
        var post = await db.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (post is null) return null!;

        post.IsDeleted = true;
        await db.SaveChangesAsync();
        return mapper.Map<PostDto>(post);
    }

    public async Task<int> CountRepliesAsync(int id)
    {
        return await db.Posts
            .AsNoTracking()
            .CountAsync(p => p.ReplyToPostId == id && !p.IsDeleted);
    }

    public async Task<IEnumerable<PostDto>> GetRepliesToPostAsync(int id)
    {
        var postsDao = await db.Posts
            .AsNoTracking()
            .Where(p => p.ReplyToPostId == id)
            .ToListAsync();
        return mapper.Map<List<PostDto>>(postsDao);
    }
}