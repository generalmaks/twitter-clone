using TwitterCloneBack.Model.Model;

namespace TwitterCloneBack.Model.Interfaces;

public interface IPostRepository
{
    Task<PostDto> GetPostByIdAsync(int id);
    Task<List<PostDto>> GetPostsAsync(int page, int pageSize);
    Task<PostDto> CreatePostAsync(PostDto postDto);
    Task<PostDto> UpdatePostAsync(PostDto postDto);
    Task<PostDto> DeletePostAsync(int id);
}
