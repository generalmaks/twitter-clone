using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneBack.Model.User.Contracts;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.User.Contracts;

namespace TwitterCloneBack.User.Controller;

[ApiController]
[Route("api/v1/users")]
public class UserController(IUserOrchestrator userOrchestrator, IMapper mapper) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetUser>> GetUserByIdAsync(int id)
    {
        return Ok(mapper.Map<GetUser>(await userOrchestrator.GetUserByIdAsync(id)));
    }

    [HttpGet]
    public async Task<ActionResult<List<GetUser>>> GetUsersAsync(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        return Ok(mapper.Map<List<GetUser>>(await userOrchestrator.GetUsersAsync(page, pageSize)));
    }

    [Authorize]
    [HttpPatch]
    public async Task<ActionResult<GetUser>> UpdateUserAsync([FromBody] UpdateUser updateUser)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var authUserId = int.Parse(userIdString!);
        if (updateUser.Id != authUserId)
            return Forbid("Trying to update different user");

        _ = await userOrchestrator.GetUserByIdAsync(authUserId);
        return Ok(mapper.Map<GetUser>(await userOrchestrator.UpdateUserAsync(updateUser)));
    }
}
