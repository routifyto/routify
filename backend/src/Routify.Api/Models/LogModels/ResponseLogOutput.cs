namespace Routify.Api.Models.LogModels;

public record ResponseLogOutput
{
    public int StatusCode { get; set; }
    public Dictionary<string, string> Headers { get; set; } = [];
    public string? Body { get; set; }
}