using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;

public class SponsorBlockProfile : Profile
{
    public SponsorBlockProfile()
    {
        CreateMap<SponsorBlock, SponsorBlockViewModel>()
            .ForMember(d => d.Sponsors, opt => opt
                .MapFrom(s => s.Sponsors.GetElements<Sponsor>()));
    }
}