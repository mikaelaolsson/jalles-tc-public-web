using Jalles.Core.ViewModels.MediaTypes;

namespace Jalles.Core.ViewModels.Blocks;

public class AttachmentBlockViewModel
{
    public IEnumerable<AttachmentViewModel> Attachments { get; set; } = Enumerable.Empty<AttachmentViewModel>();
}