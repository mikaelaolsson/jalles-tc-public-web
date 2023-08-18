using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Jalles.Core.MappingProfiles.Blocks;

public class HighlightBlockProfile : Profile
{
    public HighlightBlockProfile()
    {
        CreateMap<BlockListItem<HighlightBlock>, HighlightBlockViewModel>()
            .ForMember(d => d.Heading, opt => opt
                .MapFrom(s => s.Content.Heading))
            .ForMember(d => d.Highlights, opt => opt
                .MapFrom(s => s.Content.Highlights != null ? s.Content.Highlights.Where(h => h.ContentType.Alias == "contentPage" || h.ContentType.Alias == "listingPage") : new List<IPublishedContent>()))
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.Content.BackgroundColor.GetBackgroundColorName()));
    }
}