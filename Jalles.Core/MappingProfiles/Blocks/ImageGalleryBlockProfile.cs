using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;
public class ImageGalleryBlockProfile : Profile
{
    public ImageGalleryBlockProfile()
    {
        CreateMap<BlockListItem<ImageGalleryBlock>, ImageGalleryBlockViewModel>()
            .ForMember(d => d.Heading, opt => opt
                .MapFrom(s => s.Content.Heading))
            .ForMember(d => d.ImageGallery, opt => opt
                .MapFrom(s => s.Content.ImageGallery))
            .ForMember(d => d.Id, opt => opt
                .MapFrom(s => s.Content.Key))
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.Content.BackgroundColor.GetMediaBackgroundColor()));
    }
}
