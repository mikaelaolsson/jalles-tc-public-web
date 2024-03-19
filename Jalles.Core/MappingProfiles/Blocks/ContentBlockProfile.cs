using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;
public class ContentBlockProfile : Profile
{
    public ContentBlockProfile()
    {
        CreateMap<ContentBlock, ContentBlockViewModel>()
            .ForMember(d => d.CtaBlocks, opt => opt
                .MapFrom(s => s.Cta.GetElements<CTablock>()))
            .ForMember(d => d.Media, opt => opt
                .MapFrom(s => s.Media.GetElement<SimpleMediaBlock>()))
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.BackgroundColor.GetBackgroundColorName()));
    }
}
