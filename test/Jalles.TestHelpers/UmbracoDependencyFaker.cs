using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Media;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Jalles.TestHelpers;

[ExcludeFromCodeCoverage]
public static class UmbracoDependencyFaker
{
    public static IServiceCollection ConfigureUmbracoFakes(this IServiceCollection services)
    {
        return services
            .AddSingleton(_ => A.Fake<IPublishedValueFallback>())
            .AddSingleton(_ => A.Fake<IPublishedContentQuery>())
            .AddSingleton(_ => A.Fake<IDocumentNavigationQueryService>())
            .AddSingleton(_ => A.Fake<IPublishedUrlProvider>())
            .AddSingleton(_ => A.Fake<IImageUrlGenerator>())
            .AddSingleton(_ => A.Fake<IMapper>())
            .AddSingleton(_ => A.Fake<IMediaNavigationQueryService>());
    }
}
