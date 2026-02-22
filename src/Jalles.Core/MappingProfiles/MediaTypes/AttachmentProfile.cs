using AutoMapper;
using Jalles.Core.MappingProfiles.Resolvers;

namespace Jalles.Core.MappingProfiles.MediaTypes;

public class AttachmentProfile : Profile
{
    public AttachmentProfile()
    {
        CreateMap<Attachment, AttachmentViewModel>()
            .ForMember(d => d.FileSource, opt => opt
                .MapFrom<SourceResolver>());
    }
}
