using AutoMapper;
using Jalles.Core.Extensions;

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
