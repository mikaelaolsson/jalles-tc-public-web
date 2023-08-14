using Umbraco.Cms.Core.Strings;

namespace Jalles.Core.ViewModels.Blocks;

public class TextBlockViewModel
{
    public string? Heading { get; set; }
    public HtmlEncodedString? Text { get; set; }
    public IEnumerable<AttachmentViewModel> Attachments { get; set; } = Enumerable.Empty<AttachmentViewModel>();
}
