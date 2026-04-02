namespace Jalles.Core.ViewModels.Blocks;

public class ImageGalleryBlockViewModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Heading { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public IEnumerable<MediaWithCrops> ImageGallery { get; set; } = [];
    public IEnumerable<string> ImageGallerySources { get; set; } = [];
    public string BackgroundColor { get; set; } = "#FAFAEC";
}
