using Jalles.Core.Indexing;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;

namespace Jalles.Core.Extensions;

public static partial class UmbracoBuilderExtensions
{
    public static IUmbracoBuilder AddAsciiFoldingToExternalIndex(this IUmbracoBuilder builder)
    {
        builder.Services.ConfigureOptions<ConfigureExternalIndexOptions>();

        return builder;
    }
}
