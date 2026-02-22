using Microsoft.AspNetCore.Rewrite;
using Jalles.Web.Rules;
using Jalles.Core.Constants;

namespace Jalles.Web.Extensions;

public static partial class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseRewriteRules(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var rewriteOptions = BuildRewriteOptions();
        app.UseRewriter(rewriteOptions);

        return app;
    }

    internal static RewriteOptions BuildRewriteOptions()
    {
        return new RewriteOptions()
            .AddRedirectToWwwPermanent(JallesConstants.PublicDomainWithoutWww)
            .Add(new RedirectFromAzureWebsites());
    }
}
