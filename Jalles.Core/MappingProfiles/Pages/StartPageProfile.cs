using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;

namespace Jalles.Core.MappingProfiles.Pages;

public class StartPageProfile : Profile
{
    public StartPageProfile()
    {
        //TODO: Title can be removed
        CreateMap<StartPage, StartPageViewModel>()
            .ForMember(d => d.Title, opt => opt
                .MapFrom(s => s.Title))
            .ForMember(d => d.Thumbnail, opt => opt
                .MapFrom(s => s.Thumbnail));
    }
}
