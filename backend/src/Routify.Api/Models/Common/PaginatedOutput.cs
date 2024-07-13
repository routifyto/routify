namespace Routify.Api.Models.Common;

public record PaginatedOutput<T>
{
    public List<T> Items { get; set; } = [];
    public bool HasNext { get; set; }
    public string? NextCursor { get; set; }
}