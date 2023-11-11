using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Components;

public class FooterBlockProfile : Profile
{
    public FooterBlockProfile()
    {
        CreateMap<FooterBlock, FooterViewModel>()
            .ForMember(d => d.Sponsors, opt => opt
                .MapFrom(s => s.Sponsors.GetElements<Sponsor>()));
    }
}
