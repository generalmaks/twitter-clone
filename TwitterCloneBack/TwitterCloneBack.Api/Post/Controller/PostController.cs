using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneBack.Api.Post.Contracts;
using TwitterCloneBack.Model.Post.Contracts;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.Post.Model;

namespace TwitterCloneBack.Api.Post.Controller;

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

    [HttpPost]
    public async Task<ActionResult<PostDto>> CreatePostAsync([FromBody] PostDto postDto)
    {
        var createdPost = await postOrchestrator.CreatePostAsync(postDto);
        return Ok(createdPost);
    }

    [HttpPatch]
    public async Task<ActionResult<PostDto>> PatchPostAsync([FromBody] UpdatePost updatePost)
    {
        return Ok(await postOrchestrator.PatchPostAsync(updatePost));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<PostDto>> DeletePostAsync(int id)
    {
        return Ok(await postOrchestrator.DeletePostAsync(id));
    }
}
