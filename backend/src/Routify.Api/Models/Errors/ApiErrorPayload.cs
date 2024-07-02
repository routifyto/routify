namespace Routify.Api.Models.Errors;

public record ApiErrorPayload
{
    public ApiError Code { get; set; }
    public string Message { get; set; } = string.Empty;
}