using System.Text.Json.Serialization;

namespace Routify.Api.Models.Accounts;

public class GoogleUserInfo
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
        
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;
        
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
        
    [JsonPropertyName("picture")]
    public string Picture { get; set; } = null!;
}