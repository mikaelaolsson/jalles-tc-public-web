using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace Jalles.Web.Extensions;

public class RedirectPublicDomainsToWww : IRule
{
    private readonly string[] _publicHostNamesWithoutWww = new[] { "jallestc.se" };

    public void ApplyRule(RewriteContext context)
    {
        var request = context.HttpContext.Request;
        var host = request.Host;
        if (host.Value.StartsWith("www") || !_publicHostNamesWithoutWww.Contains(host.Host, StringComparer.OrdinalIgnoreCase))
        {
            context.Result = RuleResult.ContinueRules;
            return;
        }

        var wwwPath = request.Scheme + "://www." + host.Value + request.PathBase + request.Path + request.QueryString;

        var response = context.HttpContext.Response;
        response.StatusCode = (int)HttpStatusCode.MovedPermanently;
        response.Headers[HeaderNames.Location] = wwwPath;
        context.Result = RuleResult.EndResponse;
    }
}
