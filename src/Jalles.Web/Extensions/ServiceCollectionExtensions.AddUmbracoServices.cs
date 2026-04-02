using Jalles.Web.Contracts;
using Jalles.Web.Services;

namespace Jalles.Web.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddUmbracoServices(this IServiceCollection services)
    {
        services.AddScoped<IMixedListingBlockService, MixedListingBlockService>();
        services.AddScoped<ExcelBlockDataLoader>();
        services.AddScoped<ISearchService, SearchService>();

        return services;
    }
}
