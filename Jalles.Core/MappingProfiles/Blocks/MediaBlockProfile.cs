using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;

public class MediaBlockProfile : Profile
{
    public MediaBlockProfile()
    {
        CreateMap<MediaBlock, MediaViewModel>()
            .ForMember(d => d.IsLazy, opt => opt
                .MapFrom(_ => true))
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.BackgroundColor.GetMediaBackgroundColor()))
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource(s.GetMediaType())));

        CreateMap<IMediaProperties, MediaViewModel>()
            .ForMember(d => d.IsLazy, opt => opt
                .MapFrom(_ => true))
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.BackgroundColor.GetMediaBackgroundColor()))
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource(s.GetMediaType())));

        CreateMap<SimpleMediaBlock, MediaViewModel>()
            .ForMember(d => d.IsLazy, opt => opt
                .MapFrom(_ => true))
            .ForMember(d => d.BackgroundColor, opt => opt.Ignore())
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource(s.GetMediaType())));

        CreateMap<ISimpleMediaProperties, MediaViewModel>()
            .ForMember(d => d.IsLazy, opt => opt
                .MapFrom(_ => true))
            .ForMember(d => d.BackgroundColor, opt => opt.Ignore())
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource(s.GetMediaType())));

        CreateMap<HeaderBlock, MediaViewModel>()
            .ForMember(d => d.IsLazy, opt => opt
                .MapFrom(_ => false))
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource(s.GetMediaType())));

    }
}
