using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.MappingProfiles.Resolvers;

namespace Jalles.Core.MappingProfiles.Pages;

public class BasePageProfile : Profile
{
    public BasePageProfile()
    {
        CreateMap<IBasePageProperties, BasePageViewModel>()
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.ParentPagePath, opt => opt.Ignore())
            .ForMember(d => d.PagePath, opt => opt.Ignore())
            .ForMember(d => d.Header, opt => opt.Ignore())
            .ForMember(d => d.Thumbnail, opt => opt.Ignore());

        CreateMap<IThumbnailProperties, BasePageViewModel>()
            .ForMember(d => d.Thumbnail, opt => opt
                .MapFrom(s => s.Thumbnail))
            .ForMember(d => d.Guid, opt => opt.Ignore())
            .ForMember(d => d.PagePath, opt => opt.Ignore())
            .ForMember(d => d.ParentPagePath, opt => opt.Ignore())
            .ForMember(d => d.Title, opt => opt.Ignore())
            .ForMember(d => d.Header, opt => opt.Ignore())
            .ForMember(d => d.MetaDescription, opt => opt.Ignore());

        CreateMap<ContentPage, BasePageViewModel>()
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.ParentPagePath, opt => opt
                .MapFrom<ParentPagePathResolver<ContentPage, BasePageViewModel>>())
            .ForMember(d => d.Header, opt => opt
                .MapFrom(s => s.Header.GetElement<HeaderBlock>()))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom<PagePathResolver<ContentPage, BasePageViewModel>>());

        CreateMap<ListingPage, BasePageViewModel>()
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.ParentPagePath, opt => opt
                .MapFrom<ParentPagePathResolver<ListingPage, BasePageViewModel>>())
            .ForMember(d => d.Header, opt => opt
                .MapFrom(s => s.Header.GetElement<HeaderBlock>()))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom<PagePathResolver<ListingPage, BasePageViewModel>>());

        CreateMap<SecondaryListingPage, BasePageViewModel>()
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.ParentPagePath, opt => opt
                .MapFrom<ParentPagePathResolver<SecondaryListingPage, BasePageViewModel>>())
            .ForMember(d => d.Header, opt => opt
                .MapFrom(s => s.Header.GetElement<HeaderBlock>()))
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom<PagePathResolver<SecondaryListingPage, BasePageViewModel>>());
    }
}
