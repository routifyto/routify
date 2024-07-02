using System.Text.Json.Serialization;

namespace Routify.Api.Models.Accounts;

public class GoogleLoginInput
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = null!;
        
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = null!;
        
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}