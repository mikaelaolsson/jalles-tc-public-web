using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;
public class ContentBlockProfile : Profile
{
    public ContentBlockProfile()
    {
        CreateMap<BlockListItem<ContentBlock>, ContentBlockViewModel>()
            .ForMember(d => d.Heading, opt => opt
                .MapFrom(s => s.Content.Heading))
            .ForMember(d => d.Text, opt => opt
                .MapFrom(s => s.Content.Text))
            .ForMember(d => d.CtaBlocks, opt => opt
                .MapFrom(s => s.Content.Cta))
            .ForMember(d => d.MediaAlign, opt => opt
                .MapFrom(s => s.Content.MediaAlign))
            .ForMember(d => d.Media, opt => opt
                .MapFrom(s => s.Content.Media))
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.Content.BackgroundColor.GetBackgroundColorName()));
    }
}
