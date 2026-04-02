using AutoMapper;
using Jalles.Core.MappingProfiles.Resolvers;

namespace Jalles.Core.MappingProfiles.MediaTypes;

public class SponsorProfile : Profile
{
    public SponsorProfile()
    {
        CreateMap<Sponsor, SponsorViewModel>()
            .ForMember(d => d.Name, opt => opt
                .MapFrom(s => s.SponsorName))
            .ForMember(d => d.LogoSource, opt => opt
                .MapFrom<SourceResolver>())
            .ForMember(d => d.Website, opt => opt
                .MapFrom(s => s.SponsorWebsite));
    }
}
