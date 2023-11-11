using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;

public class ExcelBlockProfile : Profile
{
    public ExcelBlockProfile()
    {
        CreateMap<BlockListItem<ExcelBlock>, ExcelBlockViewModel>()
            .ForMember(d => d.Heading, opt => opt
                .MapFrom(s => s.Content.Heading))
            .ForMember(d => d.ExcelFile, opt => opt
                .MapFrom(s => s.Content.Excel));
    }
}