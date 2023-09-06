using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;

namespace Jalles.Core.MappingProfiles.Pages;

public class SecondaryListingPageProfile : Profile
{
    public SecondaryListingPageProfile()
    {
        CreateMap<SecondaryListingPage, SecondaryListingPageViewModel>()
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
            .ForMember(d => d.DisplayedCategories, opt => opt
                .MapFrom(s => s.DisplayedCategories.GetFilters()))
            .ForMember(d => d.MainCategory, opt => opt
                .MapFrom(s => s.MainCategory))
            .ForMember(d => d.PinThisBlock, opt => opt
                .MapFrom(s => s.Block != null && s.Block.FirstOrDefault() != null && s.Block.First().Content.ContentType.Alias == "pinThisBlock" ? s.Block.FirstOrDefault() : null))
            .ForMember(d => d.ContentPages, opt => opt
                .MapFrom(s => s.Children.OfType<ContentPage>().Where(c => c.IsVisible()).OrderByDescending(c => c.CreateDate)));
    }
}