using Jalles.Core.Constants;
using MediaType = Jalles.Core.Enum.MediaType;

namespace Jalles.Core.ViewModels.Blocks;

public class MediaBlockViewModel
{
    public string MediaSource { get; set; } = JallesConstants.DefaultFallbackMedia;
    public MediaWithCrops? Media { get; set; }
    public string? BackgroundColor { get; set; }
    public MediaType MediaType { get; set; }
    public bool IsLazy { get; set; } = true;
    public bool IsEmbeddedVideo { get; set; }
    public string AltText { get; set; } = string.Empty;
}
