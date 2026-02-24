using Jalles.Core.Contracts;
using Jalles.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Jalles.Core.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IContentAccessor, ContentAccessor>();
        services.AddScoped<IFilterService, FilterService>();
        services.AddScoped<ILayoutViewModelService, LayoutViewModelService>();
        services.AddScoped<IPaginationService, PaginationService>();
        services.AddScoped<ISitemapService, SitemapService>();
        services.AddScoped<IUmbracoPagePathService, UmbracoPagePathService>();
        services.AddScoped<IExcelService, ExcelService>();
        services.AddHttpClient<IExcelService, ExcelService>();

        return services;
    }
}
