using Microsoft.AspNetCore.ResponseCompression;

namespace Jalles.Web.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultResponseCompression(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddResponseCompression(ConfigureDefaultResponseCompressionOptions);

        return services;
    }

    internal static void ConfigureDefaultResponseCompressionOptions(ResponseCompressionOptions options)
    {
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
        options.EnableForHttps = true;
        options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["image/svg+xml"]);
    }
}
