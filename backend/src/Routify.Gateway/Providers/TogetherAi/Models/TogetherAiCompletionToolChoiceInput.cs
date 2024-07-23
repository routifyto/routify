using System.Text.Json;
using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.TogetherAi.Models;

[JsonConverter(typeof(Converter))]
internal record TogetherAiCompletionToolChoiceInput
{
    public string? StringValue { get; set; }
    public TogetherAiCompletionToolChoiceObjectInput? ObjectValue { get; set; }
    
    internal class Converter : JsonConverter<TogetherAiCompletionToolChoiceInput>
    {
        public override TogetherAiCompletionToolChoiceInput? Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new TogetherAiCompletionToolChoiceInput
                {
                    StringValue = reader.GetString()
                };
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                return new TogetherAiCompletionToolChoiceInput
                {
                    ObjectValue = JsonSerializer.Deserialize<TogetherAiCompletionToolChoiceObjectInput>(ref reader, options)
                };
            }
        
            return null;
        }

        public override void Write(
            Utf8JsonWriter writer, 
            TogetherAiCompletionToolChoiceInput value, 
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