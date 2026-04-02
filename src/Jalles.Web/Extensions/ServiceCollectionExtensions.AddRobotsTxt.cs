using Jalles.Core.Constants;
using RobotsTxt;

namespace Jalles.Web.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddRobotsTxt(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddStaticRobotsTxt(BuildJallesRobotsTxt);
        return services;
    }

    internal static RobotsTxtOptionsBuilder BuildJallesRobotsTxt(RobotsTxtOptionsBuilder robotBuilder)
    {
        return robotBuilder
            .ForEnvironment("Production")
            .ForHostnames(JallesConstants.PublicDomainWithoutWww, JallesConstants.PublicDomain)
            .AddSitemap($"https://{JallesConstants.PublicDomain}/sitemap")
            .AddSection(section =>
                section
                    .AddUserAgent("*")
                    .Disallow("/app_data/")
                    .Disallow("/app_plugins/")
                    .Disallow("/umbraco/")
                    .Disallow("/usync/")
                    .Disallow("/install")
            );
    }
}
