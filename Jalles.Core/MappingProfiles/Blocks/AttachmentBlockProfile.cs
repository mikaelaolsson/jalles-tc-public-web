using AutoMapper;
using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.MappingProfiles.Blocks;

public class AttachmentBlockProfile : Profile
{
    public AttachmentBlockProfile()
    {
        CreateMap<AttachmentBlock, AttachmentBlockViewModel>()
            .ForMember(d => d.Attachments, opt => opt
                .MapFrom(s => s.Attachments.GetElements<Attachment>()));
    }
}