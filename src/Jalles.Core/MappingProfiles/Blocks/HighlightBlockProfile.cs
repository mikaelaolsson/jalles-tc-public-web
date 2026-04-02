using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.MappingProfiles.Resolvers;

namespace Jalles.Core.MappingProfiles.Blocks;

public class HighlightBlockProfile : Profile
{
    public HighlightBlockProfile()
    {
        CreateMap<HighlightBlock, HighlightBlockViewModel>()
            .ForMember(d => d.Highlights, opt => opt
                .MapFrom<HighlightsResolver>())
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.BackgroundColor.GetBackgroundColorName()));
    }
}
