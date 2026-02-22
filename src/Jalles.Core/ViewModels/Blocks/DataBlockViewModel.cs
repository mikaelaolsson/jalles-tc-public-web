using Umbraco.Cms.Core.Strings;

namespace Jalles.Core.ViewModels.Blocks;

public class DataBlockViewModel
{
    public string Heading { get; set; } = string.Empty;
    public HtmlEncodedString Data { get; set; } = new("");
}
