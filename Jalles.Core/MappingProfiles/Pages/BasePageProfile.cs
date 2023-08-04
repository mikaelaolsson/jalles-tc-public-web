using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;

namespace Jalles.Core.MappingProfiles.Pages;

public class BasePageProfile : Profile
{
    public BasePageProfile()
    {
        CreateMap<IBasePageProperties, BasePageViewModel>()
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key));

        CreateMap<IThumbnailProperties, BasePageViewModel>()
            .ForMember(d => d.Thumbnail, opt => opt
                .MapFrom(s => s.Thumbnail));
    }
}
