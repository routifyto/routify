namespace Routify.Data.Common;

public record ResponseLog
{
    public int StatusCode { get; set; }
    public string? Body { get; set; }
    public Dictionary<string, string> Headers { get; set; } = [];
}