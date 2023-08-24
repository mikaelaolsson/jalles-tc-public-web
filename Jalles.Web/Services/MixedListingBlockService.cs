using AutoMapper;
using Jalles.Core.Contracts;
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
        return _mapper.Map<ImageGalleryBlockViewModel>(blockListItem);
    }

    public ContentBlockViewModel MapContentBlock(BlockListItem blockListItem)
    {
        return _mapper.Map<ContentBlockViewModel>(blockListItem);
    }

    public TextBlockViewModel MapTextBlock(BlockListItem blockListItem)
    {
        return _mapper.Map<TextBlockViewModel>(blockListItem);
    }

    public HighlightBlockViewModel MapHighlightBlock(BlockListItem blockListItem)
    {
        return _mapper.Map<HighlightBlockViewModel>(blockListItem);
    }

    public DataBlockViewModel MapDataBlock(BlockListItem blockListItem)
    {
        return _mapper.Map<DataBlockViewModel>(blockListItem);
    }
}