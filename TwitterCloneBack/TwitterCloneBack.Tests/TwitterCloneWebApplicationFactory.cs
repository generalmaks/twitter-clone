using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwitterCloneBack;
using TwitterCloneBack.Dal;

namespace TwitterCloneBack.Tests;

public class TwitterCloneWebApplicationFactory
    : WebApplicationFactory<Program>
{
    public TwitterCloneWebApplicationFactory()
    {
        Program.StartupType = typeof(TestStartup);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "test-jwt-key-with-enough-length-for-tests",
                ["Jwt:Issuer"] = "test-issuer",
                ["Jwt:Audience"] = "test-audience"
            });
        });
    }

    protected override void Dispose(bool disposing)
    {
        Program.StartupType = typeof(Startup);
        base.Dispose(disposing);
    }
}

public class TestStartup(IConfiguration configuration)
    : Startup(configuration)
{
    protected override void ConfigureDb(IServiceCollection services)
    {
        var dbName = $"TwitterCloneTestDb-{Guid.NewGuid()}";

        services.AddDbContext<TwitterCloneContext>(options =>
        {
            options.UseInMemoryDatabase(dbName);
        });
    }
}
