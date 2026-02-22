using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.MappingProfiles.Resolvers;

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
                .MapFrom<ParentPagePathResolver<ContentPage, ContentPageViewModel>>())
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom<PagePathResolver<ContentPage, ContentPageViewModel>>())
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
                .MapFrom<ParentPagePathResolver<ListingPage, ContentPageViewModel>>())
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom<PagePathResolver<ListingPage, ContentPageViewModel>>())
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
                .MapFrom<ParentPagePathResolver<SecondaryListingPage, ContentPageViewModel>>())
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom<PagePathResolver<SecondaryListingPage, ContentPageViewModel>>())
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
