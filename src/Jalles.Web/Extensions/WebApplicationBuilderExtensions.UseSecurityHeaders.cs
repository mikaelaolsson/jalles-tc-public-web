namespace Jalles.Web.Extensions;

public static partial class WebApplicationBuilderExtensions
{
    private static readonly string[] _contentSecurityPolicy =
                [
                    "default-src data: blob: filesystem: about: ws: wss: frame-src: * 'unsafe-inline' 'unsafe-eval';",
                    "media-src *;",
                    "script-src * data: blob: 'unsafe-inline';",
                    "connect-src * data: blob: 'unsafe-inline';",
                    "img-src * data: blob: 'unsafe-inline';",
                    "style-src * data: blob: 'unsafe-inline';",
                    "font-src * data: blob: 'unsafe-inline';",
                    "frame-ancestors * data: blob:;",
                    "object-src 'none';",
                    "form-action 'self'"
                ];

    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        return app.Use(async (context, next) =>
        {
            SetSecurityHeaders(context);
            await next();
        });
    }

    internal static void SetSecurityHeaders(HttpContext context)
    {
        var requestPath = context.Request.Path;

        context.Response.Headers.XContentTypeOptions = "nosniff";
        context.Response.Headers.XFrameOptions = "SAMEORIGIN";
        context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

        context.Response.Headers["Permissions-Policy"] =
            "geolocation=(), camera=(), microphone=(), payment=(), usb=(), fullscreen=()";

        if(!requestPath.StartsWithSegments("/umbraco") && !requestPath.StartsWithSegments("/App_Plugins"))
        {
            context.Response.Headers.ContentSecurityPolicy = string.Join(" ", _contentSecurityPolicy);
        }
    }
}
