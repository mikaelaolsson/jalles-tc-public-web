using Jalles.Core.Constants;

namespace Jalles.Web.Extensions;

public static partial class ApplicationBuilderExtensions
{
    private static readonly HashSet<string> _allowedDomains = new(StringComparer.OrdinalIgnoreCase)
    {
        JallesConstants.PublicDomain,
        JallesConstants.PublicDomainWithoutWww
    };

    public static IApplicationBuilder NoIndexOrFollow(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        return app.Use(async (context, next) =>
        {
            if(!IsPublicDomain(context.Request))
            {
                context.Response.Headers.Append("X-Robots-Tag", "noindex, nofollow");
            }

            await next();
        });
    }

    private static bool IsPublicDomain(HttpRequest request)
    {
        return _allowedDomains.Contains(request.Host.Host);
    }
}
