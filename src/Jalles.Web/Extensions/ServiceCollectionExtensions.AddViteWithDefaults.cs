using Shorthand.Vite;

namespace Jalles.Web.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddViteWithDefaults(this IServiceCollection services)
    {
        services.AddVite(options =>
        {
            options.ManifestFileName = ".vite/manifest.json";
            options.Port = 5010;
            options.Https = true;
        });

        return services;
    }
}
