using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;

namespace Jalles.Core.MappingProfiles.Pages;

public class ContentPageProfile : Profile
{
    public ContentPageProfile()
    {
        CreateMap<ContentPage, ContentPageViewModel>()
            .ForMember(d => d.Header, opt => opt
                .MapFrom(s => s.Header))
            .ForMember(d => d.Title, opt => opt
                .MapFrom(s => s.Title))
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.ParentPagePath, opt => opt
                .MapFrom(s => s.Parent!.ContentType.Alias == "startPage" ? "" : $"/{s.Parent!.UrlSegment}"))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom(s => s.UrlSegment))
            .ForMember(d => d.LastEdited, opt => opt
                .MapFrom(s => s.UpdateDate))
            .ForMember(d => d.Published, opt => opt
                .MapFrom(s => s.CreateDate))
            .ForMember(d => d.Categories, opt => opt
                .MapFrom(s => s.Categories))
            .ForMember(d => d.Blocks, opt => opt
                .MapFrom(s => s.Blocks));

        CreateMap<ListingPage, ContentPageViewModel>()
            .ForMember(d => d.Header, opt => opt
                .MapFrom(s => s.Header))
            .ForMember(d => d.Title, opt => opt
                .MapFrom(s => s.Title))
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.ParentPagePath, opt => opt
                .MapFrom(s => s.Parent!.ContentType.Alias == "startPage" ? "" : $"/{s.Parent!.UrlSegment}"))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom(s => s.UrlSegment))
            .ForMember(d => d.LastEdited, opt => opt
                .MapFrom(s => s.UpdateDate))
            .ForMember(d => d.Published, opt => opt
                .MapFrom(s => s.CreateDate))
            .ForMember(d => d.Categories, opt => opt
                .MapFrom(s => s.DisplayedCategories));

        CreateMap<SecondaryListingPage, ContentPageViewModel>()
            .ForMember(d => d.Header, opt => opt
                .MapFrom(s => s.Header))
            .ForMember(d => d.Title, opt => opt
                .MapFrom(s => s.Title))
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.ParentPagePath, opt => opt
                .MapFrom(s => s.Parent!.ContentType.Alias == "startPage" ? "" : $"/{s.Parent!.UrlSegment}"))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom(s => s.UrlSegment))
            .ForMember(d => d.LastEdited, opt => opt
                .MapFrom(s => s.UpdateDate))
            .ForMember(d => d.Published, opt => opt
                .MapFrom(s => s.CreateDate))
            .ForMember(d => d.Categories, opt => opt
                .MapFrom(s => s.DisplayedCategories));
    }
}
