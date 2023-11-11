using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;

public class DateBlockProfile : Profile
{
    public DateBlockProfile()
    {
        CreateMap<DateBlock, DateBlockViewModel>()
            .ForMember(d => d.PublishedDate, opt => opt
                .MapFrom(s => s.Date));
    }
}