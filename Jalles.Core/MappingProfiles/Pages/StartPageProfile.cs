using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;

namespace Jalles.Core.MappingProfiles.Pages;

public class StartPageProfile : Profile
{
    public StartPageProfile()
    {
        CreateMap<StartPage, StartPageViewModel>();
    }
}
