using AutoMapper;
using Jalles.Core.Contracts;
using Jalles.Core.Extensions;
using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Web.Services;

public class MixedListingBlockService : IMixedListingBlockService
{
    private readonly IMapper _mapper;

    public MixedListingBlockService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public ImageGalleryBlockViewModel MapImageGalleryBlock(BlockListItem blockListItem)
    {
        return _mapper.Map<ImageGalleryBlockViewModel>(blockListItem.GetElement<ImageGalleryBlock>());
    }

    public ContentBlockViewModel MapContentBlock(BlockListItem blockListItem)
    {
        return _mapper.Map<ContentBlockViewModel>(blockListItem.GetElement<ContentBlock>());
    }

    public TextBlockViewModel MapTextBlock(BlockListItem blockListItem)
    {
        return _mapper.Map<TextBlockViewModel>(blockListItem.GetElement<TextBlock>());
    }

    public HighlightBlockViewModel MapHighlightBlock(BlockListItem blockListItem)
    {
        return _mapper.Map<HighlightBlockViewModel>(blockListItem.GetElement<HighlightBlock>());
    }

    public DataBlockViewModel MapDataBlock(BlockListItem blockListItem)
    {
        return _mapper.Map<DataBlockViewModel>(blockListItem.GetElement<DataBlock>());
    }

    public AttachmentBlockViewModel MapAttachmentBlock(BlockListItem blockListItem)
    {
        return _mapper.Map<AttachmentBlockViewModel>(blockListItem.GetElement<AttachmentBlock>());
    }

    public SponsorBlockViewModel MapSponsorBlock(BlockListItem blockListItem)
    {
        return _mapper.Map<SponsorBlockViewModel>(blockListItem.GetElement<SponsorBlock>());
    }
}