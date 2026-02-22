using AutoMapper;

namespace Jalles.Core.MappingProfiles.Pages;

public class StartPageProfile : Profile
{
    public StartPageProfile()
    {
        CreateMap<StartPage, StartPageViewModel>();
    }
}
