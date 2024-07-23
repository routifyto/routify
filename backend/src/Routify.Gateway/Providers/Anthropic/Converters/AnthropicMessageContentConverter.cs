using System.Text.Json;
using System.Text.Json.Serialization;
using Routify.Gateway.Providers.Anthropic.Models;

namespace Routify.Gateway.Providers.Anthropic.Converters;

internal class AnthropicMessageContentConverter : JsonConverter<AnthropicCompletionMessageContentInput>
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