using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;

namespace Jalles.Core.MappingProfiles.Pages;

public class ContentPageProfile : Profile
{
    public ContentPageProfile()
    {
        CreateMap<ContentPage, ContentPageViewModel>()
            .ForMember(d => d.Title, opt => opt
                .MapFrom(s => s.Title))
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.ParentPagePath, opt => opt
                .MapFrom(s => s.Parent!.UrlSegment == "jalles" ? "" : $"/{s.Parent!.UrlSegment}"))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom(s => s.UrlSegment))
            .ForMember(d => d.LastEdited, opt => opt
                .MapFrom(s => s.UpdateDate));
    }
}
