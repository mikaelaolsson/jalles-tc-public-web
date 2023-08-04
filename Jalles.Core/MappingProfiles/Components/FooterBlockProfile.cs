using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Components;

public class FooterBlockProfile : Profile
{
    public FooterBlockProfile()
    {
        CreateMap<BlockListItem<FooterBlock>, FooterViewModel>()
            .ForMember(d => d.FooterText, opt => opt
                .MapFrom(s => s.Content.FooterText))
            .ForMember(d => d.Sponsors, opt => opt
                .MapFrom(s => s.Content.Sponsors))
            .ForMember(d => d.UmemaranLogo, opt => opt
                .MapFrom(s => s.Content.UmeMaranLogo));
    }
}
