using AutoMapper;
using Jalles.Core.MappingProfiles.Resolvers;

namespace Jalles.Core.MappingProfiles.Blocks;

public class ExcelBlockProfile : Profile
{
    public ExcelBlockProfile()
    {
        CreateMap<ExcelBlock, ExcelBlockViewModel>()
            .ForMember(d => d.ExcelFile, opt => opt
                .MapFrom(s => s.File))
            .ForMember(d => d.ExcelFileSource, opt => opt
                .MapFrom<SourceResolver>());
    }
}
