namespace Routify.Api.Models.Common;

public record ApiErrorPayload
{
    public ApiError Code { get; set; }
    public string Message { get; set; } = string.Empty;
}