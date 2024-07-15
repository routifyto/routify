namespace Routify.Data.Common;

public record RequestLog
{
    public string? Url { get; set; }
    public string? Method { get; set; }
    public Dictionary<string, string> Headers { get; set; } = [];
    public string? Body { get; set; }
}