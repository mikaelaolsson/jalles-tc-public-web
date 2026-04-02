using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Jalles.Web.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureKestrelLimits(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.WebHost.ConfigureKestrel((_, config) => ConfigureKestrelLimitsOptions(config));

        return builder;
    }

    internal static void ConfigureKestrelLimitsOptions(KestrelServerOptions config)
    {
        ArgumentNullException.ThrowIfNull(config);
        config.Limits.MaxRequestBodySize = 52_428_800 * 10;
        config.AddServerHeader = false;
    }
}
