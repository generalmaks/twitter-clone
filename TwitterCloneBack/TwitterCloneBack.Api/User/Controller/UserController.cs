using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneBack.Api.User.Contracts;
using TwitterCloneBack.Model.User.Contracts;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.Model.User.Model;

namespace TwitterCloneBack.Api.User.Controller;

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

    [HttpPost]
    public async Task<ActionResult<GetUser>> CreateUserAsync([FromBody] CreateUser createUserDto)
    {
        var createdUser = await userOrchestrator.CreateUserAsync(createUserDto);
        return Ok(mapper.Map<GetUser>(createdUser));
    }

    [HttpPatch]
    public async Task<ActionResult<GetUser>> UpdateUserAsync([FromBody] UpdateUser updateUser)
    {
        return Ok(mapper.Map<GetUser>(await userOrchestrator.UpdateUserAsync(updateUser)));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<GetUser>> DeleteUserAsync(int id)
    {
        return Ok(mapper.Map<GetUser>(await userOrchestrator.DeleteUserAsync(id)));
    }
}
