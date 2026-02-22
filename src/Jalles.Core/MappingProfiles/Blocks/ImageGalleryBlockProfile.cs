using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.MappingProfiles.Resolvers;

namespace Jalles.Core.MappingProfiles.Blocks;

public class ImageGalleryBlockProfile : Profile
{
    public ImageGalleryBlockProfile()
    {
        CreateMap<ImageGalleryBlock, ImageGalleryBlockViewModel>()
            .ForMember(d => d.Id, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.BackgroundColor.GetBackgroundColorName()))
            .ForMember(d => d.ImageGallerySources, opt => opt
                .MapFrom<SourceResolver>());
    }
}
