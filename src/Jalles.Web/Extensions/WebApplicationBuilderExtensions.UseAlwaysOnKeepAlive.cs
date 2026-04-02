namespace Jalles.Web.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAlwaysOnKeepAlive(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        return app.Use(InvokeAlwaysOnKeepAlive);
    }

    internal static async Task InvokeAlwaysOnKeepAlive(HttpContext context, Func<Task> next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        var userAgent = context.Request.Headers.UserAgent.FirstOrDefault();

        if(userAgent == "AlwaysOn")
        {
            context.Request.Path = "/keep-alive";
        }

        await next();
    }
}
