using AutoMapper;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels;

namespace Jalles.Core.MappingProfiles.MediaTypes;

public class AttachmentProfile : Profile
{
    public AttachmentProfile()
    {
        CreateMap<MediaWithCrops<Attachment>, AttachmentViewModel>()
            .ForMember(d => d.Text, opt => opt
                .MapFrom(s => s.Content.Text))
            .ForMember(d => d.File, opt => opt
                .MapFrom(s => s.Content.File));

        CreateMap<Attachment, AttachmentViewModel>()
            .ForMember(d => d.Text, opt => opt
                .MapFrom(s => s.Text))
            .ForMember(d => d.File, opt => opt
                .MapFrom(s => s.File));
    }   
}