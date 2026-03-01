using AutoMapper;
using Jalles.Core.Contracts;

namespace Jalles.Core.MappingProfiles.Resolvers;

public class HighlightsResolver
    : IValueResolver<HighlightBlock, HighlightBlockViewModel, IEnumerable<ContentPageViewModel>>
{
    private readonly IContentAccessor _contentAccessor;

    public HighlightsResolver(IContentAccessor contentAccessor)
    {
        _contentAccessor = contentAccessor;
    }

    public IEnumerable<ContentPageViewModel> Resolve(HighlightBlock source, HighlightBlockViewModel destination, IEnumerable<ContentPageViewModel> destMember,
        ResolutionContext context)
    {
        var auto = source.Auto;

        if(auto)
        {
            var contentPages = _contentAccessor
                .GetChildrenOfType<ListingPage, ContentPage>();

            if(contentPages == null)
                return [];

            return context.Mapper
                .Map<List<ContentPageViewModel>>(contentPages)
                .OrderByDescending(c => c.DateBlock?.PublishedDate ?? c.Published)
                .Take(3);
        }

        var highlights = source.Highlights;
        if(highlights == null)
            return [];

        var viewModels = new List<ContentPageViewModel>();

        foreach(var highlight in highlights)
        {
            switch(highlight.ContentType.Alias)
            {
                case "contentPage":
                    viewModels.Add(context.Mapper.Map<ContentPageViewModel>(highlight as ContentPage));
                    break;
                case "listingPage":
                    viewModels.Add(context.Mapper.Map<ContentPageViewModel>(highlight as ListingPage));
                    break;
                case "secondaryListingPage":
                    viewModels.Add(context.Mapper.Map<ContentPageViewModel>(highlight as SecondaryListingPage));
                    break;
            }
        }

        return viewModels;
    }
}
