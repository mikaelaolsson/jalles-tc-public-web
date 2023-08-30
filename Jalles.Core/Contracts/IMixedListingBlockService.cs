using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.Contracts;

public interface IMixedListingBlockService
{
    ImageGalleryBlockViewModel MapImageGalleryBlock(BlockListItem blockListItem);
    ContentBlockViewModel MapContentBlock(BlockListItem blockListItem);
    TextBlockViewModel MapTextBlock(BlockListItem blockListItem);
    HighlightBlockViewModel MapHighlightBlock(BlockListItem blockListItem);
    DataBlockViewModel MapDataBlock(BlockListItem blockListItem);
    AttachmentBlockViewModel MapAttachmentBlock(BlockListItem blockListItem);
}