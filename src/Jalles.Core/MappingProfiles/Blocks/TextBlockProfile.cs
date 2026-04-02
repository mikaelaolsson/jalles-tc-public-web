using AutoMapper;

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
