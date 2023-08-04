using Umbraco.Cms.Core.Models;
using MediaType = Jalles.Core.Enum.MediaType;

namespace Jalles.Core.ViewModels.Blocks;

public class MediaViewModel
{
    private const string _defaultFallbackMedia = "/images/jalles-media.jpg";

    public string MediaSource { get; set; } = _defaultFallbackMedia;
    public MediaWithCrops? Media { get; set; }
    public string? BackgroundColor { get; set; }
    public bool AddBlurOverlay { get; set; }
    public MediaType MediaType { get; set; }
    public bool IsLazy { get; set; } = true;
}
