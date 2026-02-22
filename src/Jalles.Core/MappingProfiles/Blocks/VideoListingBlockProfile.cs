using AutoMapper;
using Jalles.Core.Extensions;

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
