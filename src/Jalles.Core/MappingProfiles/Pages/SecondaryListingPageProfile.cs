using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.MappingProfiles.Resolvers;

namespace Jalles.Core.MappingProfiles.Pages;

public class SecondaryListingPageProfile : Profile
{
    public SecondaryListingPageProfile()
    {
        CreateMap<SecondaryListingPage, SecondaryListingPageViewModel>()
            .ForMember(d => d.Header, opt => opt
                .MapFrom(s => s.Header.GetElement<HeaderBlock>()))
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom<PagePathResolver<SecondaryListingPage, SecondaryListingPageViewModel>>())
            .ForMember(d => d.ParentPagePath, opt => opt
                .MapFrom<ParentPagePathResolver<SecondaryListingPage, SecondaryListingPageViewModel>>())
            .ForMember(d => d.DisplayedCategories, opt => opt
                .MapFrom(s => s.DisplayedCategories.GetFilters()))
            .ForMember(d => d.PinThisBlock, opt => opt
                .MapFrom(s => s.Block.GetElementByContentTypeAlias<PinThisBlock>("pinThisBlock")))
            .ForMember(d => d.ContentPages, opt => opt.Ignore())
            .ForMember(d => d.Page, opt => opt.Ignore())
            .ForMember(d => d.Pagination, opt => opt.Ignore())
            .ForMember(d => d.AllCategories, opt => opt.Ignore())
            .ForMember(d => d.SelectedCategory, opt => opt.Ignore());
    }
}
