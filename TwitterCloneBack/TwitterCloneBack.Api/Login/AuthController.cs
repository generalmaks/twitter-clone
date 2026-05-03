using Microsoft.AspNetCore.Mvc;
using TwitterCloneBack.Login.Contracts;
using TwitterCloneBack.Model.User.Interfaces;

namespace TwitterCloneBack.Login;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(IUserOrchestrator userOrchestrator, JwtTokenGenerator jwtTokenGenerator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUser login)
    {
        var user = await userOrchestrator.GetUserByEmailAsync(login.Email);

        if (!await userOrchestrator.IsCorrectPassword(user.Id, login.Password))
            return Unauthorized($"Invalid credentials");

        var token = jwtTokenGenerator.GenerateJwt(user);

        return Ok(new
        {
            userId = user.Id,
            token = token
        });
    }
}