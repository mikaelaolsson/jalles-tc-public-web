using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.MediaTypes;

namespace Jalles.Core.MappingProfiles.MediaTypes;

public class SponsorProfile : Profile
{
    public SponsorProfile()
    {
        CreateMap<Sponsor, SponsorViewModel>()
            .ForMember(d => d.Name, opt => opt
                .MapFrom(s => s.SponsorName))
            .ForMember(d => d.Logo, opt => opt
                .MapFrom(s => s.SponsorLogo))
            .ForMember(d => d.Website, opt => opt
                .MapFrom(s => s.SponsorWebsite));
    }
}
