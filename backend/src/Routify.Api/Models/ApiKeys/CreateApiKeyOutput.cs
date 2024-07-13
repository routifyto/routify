namespace Routify.Api.Models.ApiKeys;

public record CreateApiKeyOutput
{
    public string Key { get; set; } = null!;
    public ApiKeyOutput ApiKey { get; set; } = null!;
}