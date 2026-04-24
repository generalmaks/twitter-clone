using TwitterCloneBack.Model.Contracts;
using TwitterCloneBack.Model.Model;

namespace TwitterCloneBack.Model.Interfaces;

public interface IPostOrchestrator
{
    Task<PostDto> GetPostByIdAsync(int id);
    Task<List<PostDto>> GetPostsAsync(int page, int pageSize);
    Task<PostDto> CreatePostAsync(PostDto postDto);
    Task<PostDto> UpdatePostAsync(PostDto postDto);
    Task<PostDto> PatchPostAsync(UpdatePost updatePost);
    Task<PostDto> DeletePostAsync(int id);
}
