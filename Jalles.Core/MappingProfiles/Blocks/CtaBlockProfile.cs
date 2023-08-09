using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;
public class CtaBlockProfile : Profile
{
    public CtaBlockProfile()
    {
        CreateMap<BlockListItem<CTablock>, CtaBlockViewModel>()
            .ForMember(d => d.Link, opt => opt
                .MapFrom(s => s.Content.Link))
            .ForMember(d => d.Title, opt => opt
                .MapFrom(s => s.Content.Title));
    }
}
