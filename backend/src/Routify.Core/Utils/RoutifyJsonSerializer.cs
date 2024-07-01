using System.Text.Json;
using System.Text.Json.Serialization;

namespace Routify.Core.Utils;

public class RoutifyJsonSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
    public static string Serialize<TValue>(TValue value)
    {
        return JsonSerializer.Serialize(value, Options);
    }
    
    public static TValue? Deserialize<TValue>(string json)
    {
        return JsonSerializer.Deserialize<TValue>(json, Options);
    }
}