using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TwitterCloneBack.Login.Contracts;
using TwitterCloneBack.Model.User.Contracts;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.User.Contracts;

namespace TwitterCloneBack.Login;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(
    IUserOrchestrator userOrchestrator,
    JwtTokenGenerator jwtTokenGenerator,
    IMapper mapper) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser login)
    {
        var user = await userOrchestrator.GetUserByEmailAsync(login.Email);

        if (!await userOrchestrator.IsCorrectPassword(user.Id, login.Password))
            return Unauthorized("Invalid credentials");

        var token = jwtTokenGenerator.GenerateJwt(user);

        return Ok(new
        {
            userId = user.Id, token
        });
    }

    [HttpPost]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUser registerUser
    )
    {
        if (await userOrchestrator.IsUserAlreadyExistsAsync(registerUser.Email,
                registerUser.Username))
            throw new ArgumentException("User already exists");

        var user =
            await userOrchestrator.CreateUserAsync(new CreateUser
            {
                Username = registerUser.Username,
                Email = registerUser.Email,
                DisplayUsername = registerUser.DisplayUsername,
                Password = registerUser.UnhashedPassword,
                Bio = registerUser.Bio
            });

        return Ok(mapper.Map<GetUser>(user));
    }
}