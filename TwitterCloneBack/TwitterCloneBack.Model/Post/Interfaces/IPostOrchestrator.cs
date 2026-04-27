using TwitterCloneBack.Model.Post.Contracts;
using TwitterCloneBack.Model.Post.Model;

namespace TwitterCloneBack.Model.Post.Interfaces;

public interface IPostOrchestrator
{
    Task<PostDto> GetPostByIdAsync(int id);
    Task<List<PostDto>> GetPostsAsync(int page, int pageSize);
    Task<PostDto> CreatePostAsync(PostDto postDto);
    Task<PostDto> UpdatePostAsync(PostDto postDto);
    Task<PostDto> PatchPostAsync(UpdatePost updatePost);
    Task<PostDto> DeletePostAsync(int id);
}
