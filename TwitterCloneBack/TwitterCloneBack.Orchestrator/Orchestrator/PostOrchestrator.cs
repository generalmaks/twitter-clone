using TwitterCloneBack.Model.Contracts;
using TwitterCloneBack.Model.Interfaces;
using TwitterCloneBack.Model.Model;

namespace TwitterCloneBack.Orchestrator.Orchestrator;

public class PostOrchestrator(IPostRepository postRepository) : IPostOrchestrator
{
    public async Task<PostDto> GetPostByIdAsync(int id) =>
        await postRepository.GetPostByIdAsync(id) ?? throw new KeyNotFoundException($"Post with id {id} not found");

    public async Task<List<PostDto>> GetPostsAsync(int page, int pageSize) =>
        await postRepository.GetPostsAsync(page, pageSize);

    public async Task<PostDto> CreatePostAsync(PostDto postDto) =>
        await postRepository.CreatePostAsync(postDto);

    public async Task<PostDto> UpdatePostAsync(PostDto postDto) =>
        await postRepository.UpdatePostAsync(postDto) ?? throw new KeyNotFoundException($"Post with id {postDto.Id} not found");

    public async Task<PostDto> PatchPostAsync(UpdatePost updatePost)
    {
        var post = await postRepository.GetPostByIdAsync(updatePost.Id)
                   ?? throw new KeyNotFoundException($"Post with id {updatePost.Id} not found");

        post.TextContent = updatePost.TextContent ?? post.TextContent;

        return await postRepository.UpdatePostAsync(post);
    }

    public async Task<PostDto> DeletePostAsync(int id) =>
        await postRepository.DeletePostAsync(id) ?? throw new KeyNotFoundException($"Post with id {id} not found");
}
