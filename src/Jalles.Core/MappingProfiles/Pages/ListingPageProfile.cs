using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.MappingProfiles.Resolvers;

namespace Jalles.Core.MappingProfiles.Pages;

public class ListingPageProfile : Profile
{
    public ListingPageProfile()
    {
        CreateMap<ListingPage, ListingPageViewModel>()
            .ForMember(d => d.Header, opt => opt
                .MapFrom(s => s.Header.GetElement<HeaderBlock>()))
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom<PagePathResolver<ListingPage, ListingPageViewModel>>())
            .ForMember(d => d.ParentPagePath, opt => opt
                .MapFrom<ParentPagePathResolver<ListingPage, ListingPageViewModel>>())
            .ForMember(d => d.DisplayedCategories, opt => opt
                .MapFrom(s => s.DisplayedCategories.GetFilters()))
            .ForMember(d => d.AllCategories, opt => opt
                .MapFrom(s => s.Categories))
            .ForMember(d => d.ContentPages, opt => opt
                .MapFrom<ContentPagesResolver<ListingPage, ListingPageViewModel, ContentPageViewModel>>())
            .ForMember(d => d.Page, opt => opt.Ignore())
            .ForMember(d => d.Pagination, opt => opt.Ignore())
            .ForMember(d => d.SelectedCategory, opt => opt.Ignore());
    }
}
