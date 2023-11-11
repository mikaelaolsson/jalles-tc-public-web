using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;

namespace Jalles.Core.MappingProfiles.Pages;

public class ErrorPageProfile : Profile
{
    public ErrorPageProfile()
    {
        CreateMap<ErrorPage, ErrorPageViewModel>()
            .ForMember(d => d.Blocks, opt => opt
                .MapFrom(s => s.Blocks));
    }
}