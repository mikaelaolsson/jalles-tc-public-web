using AutoMapper;
using Jalles.Core.Contracts;

namespace Jalles.Core.MappingProfiles.Resolvers;

public class PagePathResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, string>
    where TSource : IPublishedContent
{
    private readonly IUmbracoPagePathService _umbracoPagePathService;

    public PagePathResolver(IUmbracoPagePathService umbracoPagePathService)
    {
        _umbracoPagePathService = umbracoPagePathService;
    }

    public string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context)
    {
        return _umbracoPagePathService.GetPagePath(source);
    }
}
