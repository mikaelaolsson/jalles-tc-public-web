using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;
using Jalles.Core.ViewModels.MediaTypes;

namespace Jalles.Core.MappingProfiles.MediaTypes;

public class AttachmentProfile : Profile
{
    public AttachmentProfile()
    {
        CreateMap<Attachment, AttachmentViewModel>();
    }   
}