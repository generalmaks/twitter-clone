using TwitterCloneBack.Model.Post.Model;

namespace TwitterCloneBack.Model.Post.Interfaces;

public interface IPostRepository
{
    Task<PostDto> GetPostByIdAsync(int id);
    Task<List<PostDto>> GetPostsAsync(int page, int pageSize);
    Task<PostDto> CreatePostAsync(PostDto postDto);
    Task<PostDto> UpdatePostAsync(PostDto postDto);
    Task<PostDto> DeletePostAsync(int id);
    Task<int> CountRepliesAsync(int id);
    Task<IEnumerable<PostDto>> GetRepliesToPostAsync(int id);

    Task<IEnumerable<PostDto>> GetPostsByTextSearchAsync(
        string search,
        int page,
        int pageSize
    );
}