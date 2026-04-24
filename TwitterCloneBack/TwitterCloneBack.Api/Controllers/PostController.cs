using Microsoft.AspNetCore.Mvc;
using TwitterCloneBack.Model.Contracts;
using TwitterCloneBack.Model.Interfaces;
using TwitterCloneBack.Model.Model;

namespace TwitterCloneBack.Controllers;

[ApiController]
[Route("api/v1/posts")]
public class PostController(IPostOrchestrator postOrchestrator) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetPostByIdAsync(int id)
    {
        return Ok(await postOrchestrator.GetPostByIdAsync(id));
    }

    [HttpGet]
    public async Task<ActionResult<List<PostDto>>> GetPostsAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        return Ok(await postOrchestrator.GetPostsAsync(page, pageSize));
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
