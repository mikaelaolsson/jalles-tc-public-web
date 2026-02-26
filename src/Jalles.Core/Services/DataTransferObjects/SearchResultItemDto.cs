namespace Jalles.Core.Services.DataTransferObjects;

public sealed record SearchResultItemDto
{
    public string Title { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public string ContentTypeTagName { get; init; } = string.Empty;
    public DateTime? UpdateDate { get; init; }
    public Uri? UriPath { get; init; }
}
