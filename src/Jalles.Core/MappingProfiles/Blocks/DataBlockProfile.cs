using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;

public class DataBlockProfile : Profile
{
    public DataBlockProfile()
    {
        CreateMap<DataBlock, DataBlockViewModel>()
            .ForMember(d => d.Data, opt => opt
                .MapFrom(s => s.DataTable!.ToHtmlString()));
    }
}