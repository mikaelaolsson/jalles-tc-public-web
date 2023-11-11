using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;
public class TextBlockProfile : Profile
{
    public TextBlockProfile()
    {
        CreateMap<BlockListItem<TextBlock>, TextBlockViewModel>()
            .ForMember(d => d.Heading, opt => opt
                .MapFrom(s => s.Content.Heading))
            .ForMember(d => d.Text, opt => opt
                .MapFrom(s => s.Content.Text));
    }
}
