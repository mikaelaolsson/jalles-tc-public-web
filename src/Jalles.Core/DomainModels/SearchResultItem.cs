namespace Jalles.Core.DomainModels;

public sealed record SearchResultItem
{
    public SearchResultItem(string title)
    {
        if(string.IsNullOrEmpty(title))
            throw new ArgumentException("Empty titles are not allowed.", nameof(title));

        Title = title;
    }

    public string Title { get; }
    public string Text { get; init; } = string.Empty;
    public string ContentTypeTagName { get; set; } = string.Empty;
    public required Uri UriPath { get; init; }
}
