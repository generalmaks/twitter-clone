using Microsoft.AspNetCore.Mvc;
using TwitterCloneBack.Model.Like.Interfaces;
using TwitterCloneBack.Model.Like.Model;

namespace TwitterCloneBack.Like.Controller;

[ApiController]
[Route("api/v1/like")]
public class LikeController(ILikeOrchestrator likeOrchestrator) : ControllerBase
{
    [HttpGet("{likeId:int}")]
    public async Task<ActionResult<LikeDto>> GetByIdAsync(int likeId)
    {
        return Ok(await likeOrchestrator.GetLikeByIdAsync(likeId));
    }

    [HttpGet("post/{postId:int}")]
    public async Task<ActionResult<List<LikeDto>>> GetLikesOnPostAsync(int postId)
    {
        return Ok(await likeOrchestrator.GetLikesOnPostAsync(postId));
    }

    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<List<LikeDto>>> GetAllLikesFromUserAsync(int userId)
    {
        return Ok(await likeOrchestrator.GetAllLikesFromUserAsync(userId));
    }

    [HttpGet("post/{postId:int}/count")]
    public async Task<ActionResult<int>> CountByPostIdAsync(int postId)
    {
        return Ok(await likeOrchestrator.CountByPostIdAsync(postId));
    }

    [HttpGet("post/{postId:int}/user/{userId:int}")]
    public async Task<ActionResult<bool>> IsPostLikedByUserAsync(int postId, int userId)
    {
        return Ok(await likeOrchestrator.IsPostLikedByUserAsync(postId, userId));
    }

    [HttpPost("{postId:int}/{userId:int}")]
    public async Task<ActionResult<LikeDto>> CreateLikeAsync(int postId, int userId)
    {
        return Ok(await likeOrchestrator.CreateLikeAsync(postId, userId));
    }

    [HttpDelete("{postId:int}/{userId:int}")]
    public async Task<ActionResult<LikeDto>> RemoveLikeAsync(int postId, int userId)
    {
        return Ok(await likeOrchestrator.RemoveLikeAsync(postId, userId));
    }
}
