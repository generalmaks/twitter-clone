using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneBack.Model.Post.Contracts;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.Post.Model;
using TwitterCloneBack.Post.Contracts;

namespace TwitterCloneBack.Post.Controller;

[ApiController]
[Route("api/v1/posts")]
public class PostController(IPostOrchestrator postOrchestrator, IMapper mapper) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetPost>> GetPostByIdAsync(int id)
    {
        return Ok(mapper.Map<GetPost>(await postOrchestrator.GetPostByIdAsync(id)));
    }

    [HttpGet]
    public async Task<ActionResult<List<GetPost>>> GetPostsAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        return Ok(mapper.Map<List<GetPost>>(await postOrchestrator.GetPostsAsync(page, pageSize)));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePostAsync([FromBody] CreatePost postDto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var authUserId = int.Parse(userIdString!);
        var post =
            new PostDto
            {
                AuthorId = authUserId,
                ReplyToPostId = postDto.ReplyToPostId,
                TextContent = postDto.TextContent,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
            };
        var createdPost = await postOrchestrator.CreatePostAsync(post);
        return Ok(createdPost);
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<PostDto>> DeletePostAsync(int id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var authUserId = int.Parse(userIdString!);
        var postAuthor = await postOrchestrator.GetPostByIdAsync(id);
        if (postAuthor.AuthorId != authUserId)
            return Forbid("Tried to delete another users post");
        return Ok(await postOrchestrator.DeletePostAsync(id));
    }

    [HttpGet("count/{id:int}")]
    public async Task<ActionResult<int>> CountRepliesAsync(int id)
    {
        return Ok(await postOrchestrator.CountRepliesAsync(id));
    }
}
