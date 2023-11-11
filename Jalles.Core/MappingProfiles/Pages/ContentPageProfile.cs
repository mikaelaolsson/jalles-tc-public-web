using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.Helpers;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;

namespace Jalles.Core.MappingProfiles.Pages;

public class ContentPageProfile : Profile
{
    public ContentPageProfile()
    {
        CreateMap<ContentPage, ContentPageViewModel>()
            .ForMember(d => d.Header, opt => opt
                .MapFrom(s => s.Header.GetElement<HeaderBlock>()))
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.ParentPagePath, opt => opt
                .MapFrom(s => s.GetParentPagePath()))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom(s => s.GetPagePath()))
            .ForMember(d => d.LastEdited, opt => opt
                .MapFrom(s => s.UpdateDate))
            .ForMember(d => d.Published, opt => opt
                .MapFrom(s => s.CreateDate))
            .ForMember(d => d.DateBlock, opt => opt
                .MapFrom(s => s.PublishedDate.GetElements<DateBlock>().FirstOrDefault()));

        CreateMap<ListingPage, ContentPageViewModel>()
            .ForMember(d => d.Header, opt => opt
                .MapFrom(s => s.Header.GetElement<HeaderBlock>()))
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.ParentPagePath, opt => opt
                .MapFrom(s => s.GetParentPagePath()))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom(s => s.GetPagePath()))
            .ForMember(d => d.LastEdited, opt => opt
                .MapFrom(s => s.UpdateDate))
            .ForMember(d => d.Published, opt => opt
                .MapFrom(s => s.CreateDate))
            .ForMember(d => d.Categories, opt => opt
                .MapFrom(s => s.DisplayedCategories))
            .ForMember(d => d.DateBlock, opt => opt.Ignore())
            .ForMember(d => d.Blocks, opt => opt.Ignore());

        CreateMap<SecondaryListingPage, ContentPageViewModel>()
            .ForMember(d => d.Header, opt => opt
                .MapFrom(s => s.Header.GetElement<HeaderBlock>()))
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.ParentPagePath, opt => opt
                .MapFrom(s => s.GetParentPagePath()))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom(s => s.GetPagePath()))
            .ForMember(d => d.LastEdited, opt => opt
                .MapFrom(s => s.UpdateDate))
            .ForMember(d => d.Published, opt => opt
                .MapFrom(s => s.CreateDate))
            .ForMember(d => d.Categories, opt => opt
                .MapFrom(s => s.DisplayedCategories))
            .ForMember(d => d.DateBlock, opt => opt.Ignore())
            .ForMember(d => d.Blocks, opt => opt.Ignore());
    }
}
