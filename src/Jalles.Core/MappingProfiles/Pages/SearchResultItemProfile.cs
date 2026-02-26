using AutoMapper;
using Jalles.Core.DomainModels;
using Jalles.Core.MappingProfiles.Resolvers;

namespace Jalles.Core.MappingProfiles.Pages;

public class SearchResultItemProfile : Profile
{
    public SearchResultItemProfile()
    {
        CreateMap<ContentPage, SearchResultItem>()
            .ForMember(d => d.Title, opt => opt
                .MapFrom(s => s.Title ?? s.Name))
            .ForMember(d => d.Text, opt => opt
                .MapFrom(s => s.MetaDescription ?? string.Empty))
            .ForMember(d => d.ContentTypeTagName, opt => opt.Ignore())
            .ForMember(d => d.UpdateDate, opt => opt
                .MapFrom(s => s.UpdateDate))
            .ForMember(d => d.UriPath, opt => opt
                .MapFrom<SearchResultUriResolver<ContentPage>>());

        CreateMap<ListingPage, SearchResultItem>()
            .ForMember(d => d.Title, opt => opt
                .MapFrom(s => s.Title ?? s.Name))
            .ForMember(d => d.Text, opt => opt
                .MapFrom(s => s.MetaDescription ?? string.Empty))
            .ForMember(d => d.ContentTypeTagName, opt => opt.Ignore())
            .ForMember(d => d.UpdateDate, opt => opt
                .MapFrom(s => s.UpdateDate))
            .ForMember(d => d.UriPath, opt => opt
                .MapFrom<SearchResultUriResolver<ListingPage>>());

        CreateMap<SecondaryListingPage, SearchResultItem>()
            .ForMember(d => d.Title, opt => opt
                .MapFrom(s => s.Title ?? s.Name))
            .ForMember(d => d.Text, opt => opt
                .MapFrom(s => s.MetaDescription ?? string.Empty))
            .ForMember(d => d.ContentTypeTagName, opt => opt.Ignore())
            .ForMember(d => d.UpdateDate, opt => opt
                .MapFrom(s => s.UpdateDate))
            .ForMember(d => d.UriPath, opt => opt
                .MapFrom<SearchResultUriResolver<SecondaryListingPage>>());

        CreateMap<StartPage, SearchResultItem>()
            .ForMember(d => d.Title, opt => opt
                .MapFrom(s => s.Title ?? s.Name))
            .ForMember(d => d.Text, opt => opt
                .MapFrom(s => s.MetaDescription ?? string.Empty))
            .ForMember(d => d.ContentTypeTagName, opt => opt.Ignore())
            .ForMember(d => d.UpdateDate, opt => opt
                .MapFrom(s => s.UpdateDate))
            .ForMember(d => d.UriPath, opt => opt
                .MapFrom<SearchResultUriResolver<StartPage>>());
    }
}
