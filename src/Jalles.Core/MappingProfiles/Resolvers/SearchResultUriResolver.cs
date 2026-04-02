using AutoMapper;
using Jalles.Core.Contracts;
using Jalles.Core.DomainModels;

namespace Jalles.Core.MappingProfiles.Resolvers;

public class SearchResultUriResolver<TSource> : IValueResolver<TSource, SearchResultItem, Uri>
    where TSource : IPublishedContent
{
    private readonly IUmbracoPagePathService _umbracoPagePathService;

    public SearchResultUriResolver(IUmbracoPagePathService umbracoPagePathService)
    {
        _umbracoPagePathService = umbracoPagePathService;
    }

    public Uri Resolve(TSource source, SearchResultItem destination, Uri destMember, ResolutionContext context)
    {
        return _umbracoPagePathService.GetPageUri(source);
    }
}
