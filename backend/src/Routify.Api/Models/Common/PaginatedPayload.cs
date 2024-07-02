namespace Routify.Api.Models.Common;

public record PaginatedPayload<T>
{
    public List<T> Items { get; set; } = [];
    public bool HasNext { get; set; }
    public string? NextCursor { get; set; }
}