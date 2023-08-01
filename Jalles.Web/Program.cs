namespace Jalles.Web;

public class Program
{
    public static string AppEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                                           ?? "Production";
    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{AppEnvironment}.json", optional: true, reloadOnChange: true)
        .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

    public static int Main(string[] args)
    {
        try
        {
            Console.WriteLine("Current process identifier: {0}", Environment.ProcessId);
            Console.WriteLine("Current environment: {0}", AppEnvironment);
            Console.WriteLine("Starting web host.");
            CreateHostBuilder(args).Build().Run();
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Host terminated unexpectedly. " + Environment.NewLine + ex.ToString());
            return 1;
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureUmbracoDefaults()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStaticWebAssets();
                webBuilder.UseStartup<Startup>();
            });
}
