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
        CreateMap<BlockListItem<MediaBlock>, MediaViewModel>()
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.Content.BackgroundColor.GetMediaBackgroundColor()))
            .ForMember(d => d.AddBlurOverlay, opt => opt
                .MapFrom(s => s.Content.AddBlurOverlay))
            .ForMember(d => d.Media, opt => opt
                .MapFrom(s => s.Content.Media))
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.Content.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.Content.GetMediaSource(s.Content.GetMediaType())));

        CreateMap<IMediaProperties, MediaViewModel>()
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.BackgroundColor.GetMediaBackgroundColor()))
            .ForMember(d => d.AddBlurOverlay, opt => opt
                .MapFrom(s => s.AddBlurOverlay))
            .ForMember(d => d.Media, opt => opt
                .MapFrom(s => s.Media))
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource(s.GetMediaType())));

        CreateMap<BlockListItem<SimpleMediaBlock>, MediaViewModel>()
            .ForMember(d => d.AddBlurOverlay, opt => opt
                .MapFrom(s => s.Content.AddBlurOverlay))
            .ForMember(d => d.Media, opt => opt
                .MapFrom(s => s.Content.Media))
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.Content.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.Content.GetMediaSource(s.Content.GetMediaType())));

        CreateMap<ISimpleMediaProperties, MediaViewModel>()
            .ForMember(d => d.AddBlurOverlay, opt => opt
                .MapFrom(s => s.AddBlurOverlay))
            .ForMember(d => d.Media, opt => opt
                .MapFrom(s => s.Media))
            .ForMember(d => d.MediaType, opt => opt
                .MapFrom(s => s.GetMediaType()))
            .ForMember(d => d.MediaSource, opt => opt
                .MapFrom(s => s.GetMediaSource(s.GetMediaType())));
    }
}
