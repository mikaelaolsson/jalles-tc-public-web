using Umbraco.Cms.Core.Media.EmbedProviders;
using Umbraco.Cms.Core.Serialization;

namespace Jalles.Core.Extensions;

public class VimeoExtensions : Vimeo
{
    public VimeoExtensions(IJsonSerializer jsonSerializer) : base(jsonSerializer)
    {
    }

    public override string GetMarkup(string url, int maxWidth, int maxHeight)
    {
        var markup = base.GetMarkup(url, maxWidth, maxHeight);
        return "<div class=\"embedded-video-container\">" + markup + "</div>";
    }
}
