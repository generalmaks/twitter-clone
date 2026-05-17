using Microsoft.AspNetCore.Mvc;

namespace TwitterCloneBack;

[ApiController]
[Route("api/v1/healthcheck")]
public class Healthcheck : ControllerBase
{
    [HttpGet]
    public Task<ActionResult<ServerHealth>> HealthCheck()
    {
        return Task.FromResult<ActionResult<ServerHealth>>(Ok(new ServerHealth(DateTime.UtcNow, "fine")));
    }
}

public record ServerHealth(DateTime Time, string Status);