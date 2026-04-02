using AutoMapper;
using Jalles.Core.Extensions;

namespace Jalles.Core.MappingProfiles.Blocks;

public class PinThisBlockProfile : Profile
{
    public PinThisBlockProfile()
    {
        CreateMap<PinThisBlock, PinThisBlockViewModel>()
            .ForMember(d => d.Pins, opt => opt
                .MapFrom(s => s.Pins.GetElements<ContentPage>()));
    }
}
