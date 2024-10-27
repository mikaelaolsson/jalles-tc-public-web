using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;
public class VideoListingBLockProfile : Profile
{
    public VideoListingBLockProfile()
    {
        CreateMap<VideoListingBlock, VideoListingBlockViewModel>()
            .ForMember(dest => dest.Videos, opt => opt
                .MapFrom(src => src.Videos.GetElements<IVideoBlockProperties>()));
    }
}
