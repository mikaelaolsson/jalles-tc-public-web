using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;

public class DataBlockProfile : Profile
{
    public DataBlockProfile()
    {
        CreateMap<BlockListItem<DataBlock>, DataBlockViewModel>()
            .ForMember(d => d.Heading, opt => opt
                .MapFrom(s => s.Content.Heading))
            .ForMember(d => d.Data, opt => opt
                .MapFrom(s => s.Content.DataTable));
    }
}