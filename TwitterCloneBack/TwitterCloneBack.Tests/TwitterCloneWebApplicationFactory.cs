using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TwitterCloneBack.Dal;

namespace TwitterCloneBack.Tests;

public class TwitterCloneWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextOptionsDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<TwitterCloneContext>));
            if (dbContextOptionsDescriptor != null)
            {
                services.Remove(dbContextOptionsDescriptor);
            }

            var ctxDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(TwitterCloneContext));
            if (ctxDescriptor != null)
            {
                services.Remove(ctxDescriptor);
            }

            var genericOptionsDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions));
            if (genericOptionsDescriptor != null)
            {
                services.Remove(genericOptionsDescriptor);
            }

            var dbName = $"TwitterCloneTestDb-{Guid.NewGuid()}";
            
            services.AddEntityFrameworkInMemoryDatabase();

            services.AddDbContext<TwitterCloneContext>((sp, options) =>
            {
                options.UseInMemoryDatabase(dbName)
                       .UseInternalServiceProvider(sp);
            });
        });
    }
}
