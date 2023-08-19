using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Jalles.Core.MappingProfiles.Blocks;

public class PinThisBlockProfile : Profile
{
    public PinThisBlockProfile()
    {
        CreateMap<BlockListItem<PinThisBlock>, PinThisBlockViewModel>()
            .ForMember(d => d.Pins, opt => opt
                .MapFrom(s =>
                    s.Content.Pins != null
                        ? s.Content.Pins.Where(h => h.ContentType.Alias == "contentPage")
                        : new List<IPublishedContent>()));
    }
}