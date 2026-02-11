using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using MediaType = Jalles.Core.Enum.MediaType;

namespace Jalles.Core.MappingProfiles.Blocks;

public class MediaBlockProfile : Profile
{
    public MediaBlockProfile()
    {
        CreateMap<MediaBlock, MediaBlockViewModel>()
            .ForMember(d => d.IsEmbeddedVideo, opt => opt
                .Ignore())
            .ForMember(d => d.AltText, opt => opt
                .Ignore())
            .ForMember(d => d.IsLazy, opt => opt
                .MapFrom(_ => true))
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.BackgroundColor.GetMediaBackgroundColor()))
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource(s.GetMediaType())));

        CreateMap<IMediaProperties, MediaBlockViewModel>()
            .ForMember(d => d.IsEmbeddedVideo, opt => opt
                .Ignore())
            .ForMember(d => d.AltText, opt => opt
                .Ignore())
            .ForMember(d => d.IsLazy, opt => opt
                .MapFrom(_ => true))
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.BackgroundColor.GetMediaBackgroundColor()))
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource(s.GetMediaType())));

        CreateMap<SimpleMediaBlock, MediaBlockViewModel>()
            .ForMember(d => d.IsEmbeddedVideo, opt => opt
                .Ignore())
            .ForMember(d => d.AltText, opt => opt
                .Ignore())
            .ForMember(d => d.IsLazy, opt => opt
                .MapFrom(_ => true))
            .ForMember(d => d.BackgroundColor, opt => opt.Ignore())
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource(s.GetMediaType())));

        CreateMap<ISimpleMediaProperties, MediaBlockViewModel>()
            .ForMember(d => d.IsEmbeddedVideo, opt => opt
                .Ignore())
            .ForMember(d => d.AltText, opt => opt
                .Ignore())
            .ForMember(d => d.IsLazy, opt => opt
                .MapFrom(_ => true))
            .ForMember(d => d.BackgroundColor, opt => opt.Ignore())
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource(s.GetMediaType())));

        CreateMap<HeaderBlock, MediaBlockViewModel>()
            .ForMember(d => d.IsEmbeddedVideo, opt => opt
                .Ignore())
            .ForMember(d => d.AltText, opt => opt
                .Ignore())
            .ForMember(d => d.IsLazy, opt => opt
                .MapFrom(_ => false))
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource(s.GetMediaType())));

        CreateMap<IVideoBlockProperties, MediaBlockViewModel>()
            .ForMember(d => d.Media, opt => opt
                .Ignore())
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(_ => MediaType.Video))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource()))
            .ForMember(d => d.IsEmbeddedVideo, opt => opt
                .MapFrom(_ => true))
            .ForMember(d => d.IsLazy, opt => opt
                .Ignore());

        CreateMap<VideoBlock, MediaBlockViewModel>()
            .ForMember(d => d.Media, opt => opt
                .Ignore())
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(_ => MediaType.Video))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource()))
            .ForMember(d => d.IsEmbeddedVideo, opt => opt
                .MapFrom(_ => true))
            .ForMember(d => d.IsLazy, opt => opt
                .Ignore());
    }
}
