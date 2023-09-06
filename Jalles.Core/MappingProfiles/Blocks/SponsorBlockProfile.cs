using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;

public class SponsorBlockProfile : Profile
{
    public SponsorBlockProfile()
    {
        CreateMap<BlockListItem<SponsorBlock>, SponsorBlockViewModel>()
            .ForMember(d => d.Heading, opt => opt
                .MapFrom(s => s.Content.Heading))
            .ForMember(d => d.Sponsors, opt => opt
                .MapFrom(s => s.Content.Sponsors));
    }
}