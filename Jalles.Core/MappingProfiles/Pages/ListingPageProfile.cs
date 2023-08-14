using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;

namespace Jalles.Core.MappingProfiles.Pages;

public class ListingPageProfile : Profile
{
    public ListingPageProfile()
    {
        CreateMap<ListingPage, ListingPageViewModel>()
            .ForMember(d => d.Header, opt => opt
                .MapFrom(s => s.Header))
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.Title, opt => opt
                .MapFrom(s => s.Title))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom(s => s.UrlSegment))
            .ForMember(d => d.DisplayTitle, opt => opt
                .MapFrom(s => s.DisplayTitle))
            .ForMember(d => d.ContentPages, opt => opt
                .MapFrom(s => s.Children.OfType<ContentPage>().Where(c => c.IsVisible()).OrderByDescending(c => c.CreateDate)));
    }
}
