using System.Text.Json;
using System.Text.Json.Serialization;

namespace Routify.Core.Utils;

public class RoutifyJsonSerializer
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper),
            new JsonObjectConverter()
        }
    };
    
    public static string Serialize<TValue>(TValue value)
    {
        return JsonSerializer.Serialize(value, _options);
    }
    
    public static TValue? Deserialize<TValue>(string json)
    {
        return JsonSerializer.Deserialize<TValue>(json, _options);
    }
}