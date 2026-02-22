using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Web.Contracts;

public interface IMixedListingBlockService
{
    ImageGalleryBlockViewModel MapImageGalleryBlock(BlockListItem blockListItem);
    ContentBlockViewModel MapContentBlock(BlockListItem blockListItem);
    TextBlockViewModel MapTextBlock(BlockListItem blockListItem);
    HighlightBlockViewModel MapHighlightBlock(BlockListItem blockListItem);
    DataBlockViewModel MapDataBlock(BlockListItem blockListItem);
    AttachmentBlockViewModel MapAttachmentBlock(BlockListItem blockListItem);
    SponsorBlockViewModel MapSponsorBlock(BlockListItem blockListItem);
    ExcelBlockViewModel MapExcelBlock(BlockListItem blockListItem);
    VideoListingBlockViewModel MapVideoListingBlock(BlockListItem blockListItem);
}
