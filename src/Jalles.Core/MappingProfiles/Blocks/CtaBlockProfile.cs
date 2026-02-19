using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;
public class CtaBlockProfile : Profile
{
    public CtaBlockProfile()
    {
        CreateMap<CTablock, CtaBlockViewModel>();
    }
}
