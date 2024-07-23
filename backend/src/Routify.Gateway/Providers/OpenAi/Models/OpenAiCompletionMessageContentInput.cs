using System.Text.Json;
using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.OpenAi.Models;

[JsonConverter(typeof(Converter))]
internal record OpenAiCompletionMessageContentInput
{
    public string? StringValue { get; set; }
    public List<OpenAiCompletionMessageContentPartInput>? ListValue { get; set; }
    
    internal class Converter : JsonConverter<OpenAiCompletionMessageContentInput>
    {
        public override OpenAiCompletionMessageContentInput? Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new OpenAiCompletionMessageContentInput
                {
                    StringValue = reader.GetString()
                };
            }

            if (reader.TokenType == JsonTokenType.StartArray)
            {
                return new OpenAiCompletionMessageContentInput
                {
                    ListValue = JsonSerializer.Deserialize<List<OpenAiCompletionMessageContentPartInput>>(ref reader, options)
                };
            }
        
            return null;
        }

        public override void Write(
            Utf8JsonWriter writer, 
            OpenAiCompletionMessageContentInput value, 
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