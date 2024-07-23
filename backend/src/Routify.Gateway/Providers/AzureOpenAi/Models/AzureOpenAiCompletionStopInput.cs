using System.Text.Json;
using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.AzureOpenAi.Models;

[JsonConverter(typeof(Converter))]
internal record AzureOpenAiCompletionStopInput
{
    public string? StringValue { get; set; }
    public List<string>? ListValue { get; set; }
    
    internal class Converter : JsonConverter<AzureOpenAiCompletionStopInput>
    {
        public override AzureOpenAiCompletionStopInput Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new AzureOpenAiCompletionStopInput { StringValue = reader.GetString() };
            }

            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var list = JsonSerializer.Deserialize<List<string>>(ref reader, options);
                return new AzureOpenAiCompletionStopInput { ListValue = list };
            }

            throw new JsonException();
        }

        public override void Write(
            Utf8JsonWriter writer, 
            AzureOpenAiCompletionStopInput value, 
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
            else
            {
                throw new JsonException();
            }
        }
    }
}