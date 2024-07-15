namespace Routify.Api.Models.LogModels;

public record RequestLogOutput
{
    public string? Url { get; set; }
    public string? Method { get; set; }
    public Dictionary<string, string> Headers { get; set; } = [];
    public string? Body { get; set; }
}