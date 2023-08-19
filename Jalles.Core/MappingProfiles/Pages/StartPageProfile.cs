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
            .ForMember(d => d.Blocks, opt => opt
                .MapFrom(s => s.Blocks))
            .ForMember(d => d.Thumbnail, opt => opt
                .MapFrom(s => s.Thumbnail));
    }
}
