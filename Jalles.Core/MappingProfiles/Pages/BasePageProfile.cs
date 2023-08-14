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

        CreateMap<ContentPage, BasePageViewModel>()
            .ForMember(d => d.Title, opt => opt
                .MapFrom(s => s.Title))
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom(s => s.UrlSegment));

        CreateMap<ListingPage, BasePageViewModel>()
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.Title, opt => opt
                .MapFrom(s => s.Title))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom(s => s.UrlSegment));
    }
}
