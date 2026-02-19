namespace Jalles.Core.ViewModels.Blocks;

public class VideoListingBlockViewModel
{
    public string? Heading { get; init; }
    public string? Description { get; init; }
    public IEnumerable<MediaBlockViewModel> Videos { get; init; } = [];
}
