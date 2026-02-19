using File = System.IO.File;

namespace Jalles.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseRobotsTxt(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        return app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/robots.txt"))
            {
                var robotsTxtPath = Path.Combine(env.ContentRootPath, "robots.txt");
                var output = "User-agent: *  \nDisallow: /";

                if (IsPublicDomain(context.Request))
                {
                    if (!File.Exists(robotsTxtPath))
                    {
                        throw new Exception($"robots.txt file is missing on path {robotsTxtPath}.");
                    }

                    output = await File.ReadAllTextAsync(robotsTxtPath);
                }

                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(output);
            }
            else
            {
                await next();
            }
        });
    }

    private static bool IsPublicDomain(HttpRequest contextRequest)
    {
        return contextRequest.Host.Host is "jallestc.se" or "www.jallestc.se";
    }

    public static IApplicationBuilder NoIndexOrFollow(this IApplicationBuilder app, IWebHostEnvironment _)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        return app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("X-Robots-Tag", "noindex, nofollow");
            await next();
        });
    }
}
