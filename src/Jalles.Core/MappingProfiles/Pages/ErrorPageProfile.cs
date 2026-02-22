using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.MappingProfiles.Resolvers;

namespace Jalles.Core.MappingProfiles.Pages;

public class ErrorPageProfile : Profile
{
    public ErrorPageProfile()
    {
        CreateMap<ErrorPage, ErrorPageViewModel>()
            .ForMember(d => d.Header, opt => opt
                .MapFrom(s => s.Header.GetElement<HeaderBlock>()))
            .ForMember(d => d.Guid, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.ParentPagePath, opt => opt
                .MapFrom<ParentPagePathResolver<ErrorPage, ErrorPageViewModel>>())
            .ForMember(d => d.PagePath, opt => opt
                .MapFrom<PagePathResolver<ErrorPage, ErrorPageViewModel>>())
            .ForMember(d => d.LastEdited, opt => opt
                .MapFrom(s => s.UpdateDate))
            .ForMember(d => d.Published, opt => opt
                .MapFrom(s => s.CreateDate))
            .ForMember(d => d.Blocks, opt => opt
                .MapFrom(s => s.Blocks))
            .ForMember(d => d.DateBlock, opt => opt.Ignore())
            .ForMember(d => d.Categories, opt => opt.Ignore())
            .ForMember(d => d.Title, opt => opt.Ignore())
            .ForMember(d => d.Thumbnail, opt => opt.Ignore())
            .ForMember(d => d.MetaDescription, opt => opt.Ignore());
    }
}
