namespace TwitterCloneBack;

public class Program
{
    public static Type StartupType { get; set; } = typeof(Startup);

    public static void Main(string[] args)
    {
        CreateHostBuilder(args)
            .Build()
            .Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup(StartupType);
            });
    }
}
