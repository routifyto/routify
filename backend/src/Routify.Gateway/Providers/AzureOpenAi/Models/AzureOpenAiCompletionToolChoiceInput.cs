using System.Text.Json;
using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.AzureOpenAi.Models;

[JsonConverter(typeof(Converter))]
internal record AzureOpenAiCompletionToolChoiceInput
{
    public string? StringValue { get; set; }
    public AzureOpenAiCompletionToolChoiceObjectInput? ObjectValue { get; set; }
    
    internal class Converter : JsonConverter<AzureOpenAiCompletionToolChoiceInput>
    {
        public override AzureOpenAiCompletionToolChoiceInput? Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new AzureOpenAiCompletionToolChoiceInput
                {
                    StringValue = reader.GetString()
                };
            }

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                return new AzureOpenAiCompletionToolChoiceInput
                {
                    ObjectValue = JsonSerializer.Deserialize<AzureOpenAiCompletionToolChoiceObjectInput>(ref reader, options)
                };
            }
        
            return null;
        }

        public override void Write(
            Utf8JsonWriter writer, 
            AzureOpenAiCompletionToolChoiceInput value, 
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