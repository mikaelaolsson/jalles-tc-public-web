namespace Jalles.Core.Models.Domain;

public class SitemapEntry
{
    public string Loc { get; set; } = string.Empty;
    public DateTimeOffset LastMod { get; set; }
}
