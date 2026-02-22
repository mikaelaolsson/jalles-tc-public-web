using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.MappingProfiles.Resolvers;

namespace Jalles.Core.MappingProfiles.Components;

public class FooterBlockProfile : Profile
{
    public FooterBlockProfile()
    {
        CreateMap<FooterBlock, FooterViewModel>()
            .ForMember(d => d.Sponsors, opt => opt
                .MapFrom(s => s.Sponsors.GetElements<Sponsor>()))
            .ForMember(d => d.UmemaranLogoSource, opt => opt
                .MapFrom<SourceResolver>());
    }
}
