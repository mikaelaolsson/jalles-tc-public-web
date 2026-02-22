using AutoMapper;
using Jalles.Core.Extensions;

namespace Jalles.Core.MappingProfiles.Blocks;

public class HighlightBlockProfile : Profile
{
    public HighlightBlockProfile()
    {
        CreateMap<HighlightBlock, HighlightBlockViewModel>()
            .ForMember(d => d.Highlights, opt => opt
                .MapFrom<HighlightsResolver>())
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.BackgroundColor.GetBackgroundColorName()));
    }
}

public class HighlightsResolver
    : IValueResolver<HighlightBlock, HighlightBlockViewModel, IEnumerable<ContentPageViewModel>>
{
    public IEnumerable<ContentPageViewModel> Resolve(HighlightBlock source, HighlightBlockViewModel destination, IEnumerable<ContentPageViewModel> destMember,
        ResolutionContext context)
    {
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
