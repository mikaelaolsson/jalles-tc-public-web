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
        CreateMap<ImageGalleryBlock, ImageGalleryBlockViewModel>()
            .ForMember(d => d.Id, opt => opt
                .MapFrom(s => s.Key))
            .ForMember(d => d.BackgroundColor, opt => opt
                .MapFrom(s => s.BackgroundColor.GetBackgroundColorName()));
    }
}
