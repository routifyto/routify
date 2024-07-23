using System.Text.Json;
using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Groq.Models;

[JsonConverter(typeof(Converter))]
internal record GroqCompletionToolChoiceInput
{
    public string? StringValue { get; set; }
    public GroqCompletionToolChoiceObjectInput? ObjectValue { get; set; }
    
    internal class Converter : JsonConverter<GroqCompletionToolChoiceInput>
    {
        public override GroqCompletionToolChoiceInput? Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new GroqCompletionToolChoiceInput
                {
                    StringValue = reader.GetString()
                };
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                return new GroqCompletionToolChoiceInput
                {
                    ObjectValue = JsonSerializer.Deserialize<GroqCompletionToolChoiceObjectInput>(ref reader, options)
                };
            }
        
            return null;
        }

        public override void Write(
            Utf8JsonWriter writer, 
            GroqCompletionToolChoiceInput value, 
            JsonSerializerOptions options)
        {
            if (value.StringValue != null)
            {
                writer.WriteStringValue(value.StringValue);
            }
            else if (value.ObjectValue != null)
            {
                JsonSerializer.Serialize(writer, value.ObjectValue, options);
            }
        }
    }
}