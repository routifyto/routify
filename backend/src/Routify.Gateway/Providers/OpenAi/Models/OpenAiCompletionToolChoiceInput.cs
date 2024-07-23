using System.Text.Json;
using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.OpenAi.Models;

[JsonConverter(typeof(Converter))]
internal record OpenAiCompletionToolChoiceInput
{
    public string? StringValue { get; set; }
    public OpenAiCompletionToolChoiceObjectInput? ObjectValue { get; set; }
    
    internal class Converter : JsonConverter<OpenAiCompletionToolChoiceInput>
    {
        public override OpenAiCompletionToolChoiceInput? Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new OpenAiCompletionToolChoiceInput
                {
                    StringValue = reader.GetString()
                };
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                return new OpenAiCompletionToolChoiceInput
                {
                    ObjectValue = JsonSerializer.Deserialize<OpenAiCompletionToolChoiceObjectInput>(ref reader, options)
                };
            }
        
            return null;
        }

        public override void Write(
            Utf8JsonWriter writer, 
            OpenAiCompletionToolChoiceInput value, 
            JsonSerializerOptions options)
        {
            if (value.StringValue != null)
            {
                writer.WriteStringValue(value.StringValue);
            }
            else
            {
                JsonSerializer.Serialize(writer, value.ObjectValue, options);
            }
        }
    }
}