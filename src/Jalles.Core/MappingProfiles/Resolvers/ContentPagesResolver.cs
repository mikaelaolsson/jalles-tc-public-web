using AutoMapper;
using Jalles.Core.Contracts;
using Umbraco.Extensions;

namespace Jalles.Core.MappingProfiles.Resolvers;

public class ContentPagesResolver<TSource, TDestination, TDestChild> : IValueResolver<TSource, TDestination, IEnumerable<TDestChild>>
    where TSource : IPublishedContent
{
    private readonly IContentAccessor _contentAccessor;
    private readonly IMapper _mapper;

    public ContentPagesResolver(IContentAccessor contentAccessor, IMapper mapper)
    {
        _contentAccessor = contentAccessor;
        _mapper = mapper;
    }

    public IEnumerable<TDestChild> Resolve(TSource source, TDestination destination, IEnumerable<TDestChild> destMember, ResolutionContext context)
    {
        var children = _contentAccessor
            .GetChildrenOfTypeFromParent<ContentPage>(source)
            .Where(c => c.IsVisible())
            .OrderByDescending(c => c.CreateDate);

        return _mapper.Map<IEnumerable<TDestChild>>(children);
    }
}
