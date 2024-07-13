namespace Routify.Api.Models.Common;

public record ApiErrorOutput
{
    public ApiError Code { get; set; }
    public string Message { get; set; } = string.Empty;
}