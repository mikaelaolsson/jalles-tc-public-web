namespace Jalles.Web.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    public static IApplicationBuilder UseStaticAssetCacheControl(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        return app.Use(InvokeStaticAssetCacheControl);
    }

    internal static async Task InvokeStaticAssetCacheControl(HttpContext context, Func<Task> next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        var path = context.Request.Path.Value;
        if(path?.StartsWith("/umbraco/") != false)
        {
            await next();

            return;
        }

        var cachableExtensions = new[] { ".js", ".css", ".woff", ".woff2", ".svgz", ".svg" };
        if(cachableExtensions.Any(path.EndsWith) || path.StartsWith("/media/"))
        {
            context.Response.Headers.CacheControl = "public, max-age=31536000";
        }

        await next();
    }
}
