using AutoMapper;
using Jalles.Core.Extensions;

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
