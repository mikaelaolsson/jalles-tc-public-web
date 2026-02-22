using Jalles.Core.Constants;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace Jalles.Web.Rules;

public class RedirectFromAzureWebsites : IRule
{
    public void ApplyRule(RewriteContext context)
    {
        var request = context.HttpContext.Request;
        var host = request.Host;

        if(host.Host.EndsWith("jalles-tc-public-web.azurewebsites.net"))
        {
            Redirect(context, JallesConstants.PublicDomain);
        }
        else
        {
            context.Result = RuleResult.ContinueRules;
        }
    }

    private static void Redirect(RewriteContext context, string newHostName)
    {
        var request = context.HttpContext.Request;

        var uriBuilder = new UriBuilder(request.Scheme, newHostName)
        {
            Path = request.PathBase + request.Path,
            Query = request.QueryString.ToString()
        };

        var redirectUrl = uriBuilder.Uri.ToString();

        var response = context.HttpContext.Response;
        response.StatusCode = (int)HttpStatusCode.MovedPermanently;
        response.Headers[HeaderNames.Location] = redirectUrl;
        context.Result = RuleResult.EndResponse;
    }
}
