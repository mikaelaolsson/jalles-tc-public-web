namespace Jalles.Core.Services.DataTransferObjects;

public sealed record SearchResultDto
{
    public int TotalNumberOfItems { get; init; }
    public IEnumerable<SearchResultItemDto> SearchResultItems { get; init; } = [];
}
