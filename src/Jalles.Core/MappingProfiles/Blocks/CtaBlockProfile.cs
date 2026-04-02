using AutoMapper;

namespace Jalles.Core.MappingProfiles.Blocks;

public class CtaBlockProfile : Profile
{
    public CtaBlockProfile()
    {
        CreateMap<CTablock, CtaBlockViewModel>();
    }
}
