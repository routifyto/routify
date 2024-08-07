using System.Text.Json;
using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Anthropic.Models;

[JsonConverter(typeof(Converter))]
internal record AnthropicCompletionMessageContentInput
{
    public string? StringValue { get; set; }
    public List<AnthropicCompletionMessageContentBlockInput>? ListValue { get; set; }
    
    internal class Converter : JsonConverter<AnthropicCompletionMessageContentInput>
    {
        public override AnthropicCompletionMessageContentInput? Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new AnthropicCompletionMessageContentInput
                {
                    StringValue = reader.GetString()
                };
            }

            if (reader.TokenType == JsonTokenType.StartArray)
            {
                return new AnthropicCompletionMessageContentInput
                {
                    ListValue = JsonSerializer.Deserialize<List<AnthropicCompletionMessageContentBlockInput>>(ref reader, options)
                };
            }
        
            return null;
        }

        public override void Write(
            Utf8JsonWriter writer, 
            AnthropicCompletionMessageContentInput value, 
            JsonSerializerOptions options)
        {
            if (value.StringValue != null)
            {
                writer.WriteStringValue(value.StringValue);
            }
            else if (value.ListValue != null)
            {
                JsonSerializer.Serialize(writer, value.ListValue, options);
            }
        }
    }
}