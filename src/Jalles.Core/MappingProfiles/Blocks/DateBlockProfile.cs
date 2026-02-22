using AutoMapper;

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
