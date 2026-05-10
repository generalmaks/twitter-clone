using TwitterCloneBack.Model.Post.Contracts;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.Post.Model;

namespace TwitterCloneBack.Orchestrator.Post.Orchestrator;

public class PostOrchestrator(
    IPostRepository postRepository) : IPostOrchestrator
{
    public async Task<PostDto> GetPostByIdAsync(int id)
    {
        return await postRepository.GetPostByIdAsync(id) ??
               throw new NotFoundException($"Post with id {id} not found");
    }

    public async Task<List<PostDto>> GetPostsAsync(int page, int pageSize)
    {
        return await postRepository.GetPostsAsync(page, pageSize);
    }

    public async Task<PostDto> CreatePostAsync(PostDto postDto)
    {
        return await postRepository.CreatePostAsync(postDto);
    }

    public async Task<PostDto> UpdatePostAsync(PostDto postDto)
    {
        return await postRepository.UpdatePostAsync(postDto) ??
               throw new NotFoundException(
                   $"Post with id {postDto.Id} not found");
    }

    public async Task<PostDto> PatchPostAsync(UpdatePost updatePost)
    {
        var post =
            await postRepository.GetPostByIdAsync(updatePost.Id)
            ?? throw new NotFoundException(
                $"Post with id {updatePost.Id} not found");

        post.TextContent = updatePost.TextContent ?? post.TextContent;

        return await postRepository.UpdatePostAsync(post);
    }

    public async Task<PostDto> DeletePostAsync(int id)
    {
        return await postRepository.DeletePostAsync(id) ??
               throw new NotFoundException($"Post with id {id} not found");
    }

    public async Task<int> CountRepliesAsync(int id)
    {
        _ = await GetPostByIdAsync(id);
        return await postRepository.CountRepliesAsync(id);
    }

    public async Task<IEnumerable<PostDto>> GetRepliesToPostAsync(int id)
    {
        _ = await GetPostByIdAsync(id);
        return await postRepository.GetRepliesToPostAsync(id);
    }
}
