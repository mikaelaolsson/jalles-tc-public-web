using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;

namespace Jalles.Core.MappingProfiles.MediaTypes;

public class SponsorProfile : Profile
{
    public SponsorProfile()
    {
        CreateMap<MediaWithCrops<Sponsor>, SponsorViewModel>()
            .ForMember(d => d.Name, opt => opt
                .MapFrom(s => s.Content.SponsorName))
            .ForMember(d => d.Logo, opt => opt
                .MapFrom(s => s.Content.SponsorLogo))
            .ForMember(d => d.Website, opt => opt
                .MapFrom(s => s.Content.SponsorWebsite));

        CreateMap<Sponsor, SponsorViewModel>()
            .ForMember(d => d.Name, opt => opt
                .MapFrom(s => s.SponsorName))
            .ForMember(d => d.Logo, opt => opt
                .MapFrom(s => s.SponsorLogo))
            .ForMember(d => d.Website, opt => opt
                .MapFrom(s => s.SponsorWebsite));
    }
}
