using Microsoft.AspNetCore.HttpOverrides;

namespace Jalles.Web.Extensions;

public static partial class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseTrustedForwardedHeaders(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var forwardedHeaderOptions = BuildTrustedForwardedHeadersOptions();
        app.UseForwardedHeaders(forwardedHeaderOptions);

        return app;
    }

    internal static ForwardedHeadersOptions BuildTrustedForwardedHeadersOptions()
    {
        var options = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        };

        options.KnownIPNetworks.Clear();
        options.KnownProxies.Clear();

        return options;
    }
}
