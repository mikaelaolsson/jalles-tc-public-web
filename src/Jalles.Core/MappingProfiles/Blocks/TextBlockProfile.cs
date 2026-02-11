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
        CreateMap<TextBlock, TextBlockViewModel>()
            .ForMember(d => d.Text, opt => opt
                .MapFrom(s => s.Text!.ToHtmlString()));
    }
}
