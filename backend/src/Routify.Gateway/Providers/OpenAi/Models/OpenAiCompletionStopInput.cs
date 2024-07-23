using System.Text.Json;
using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.OpenAi.Models;

[JsonConverter(typeof(Converter))]
internal record OpenAiCompletionStopInput
{
    public string? StringValue { get; set; }
    public List<string>? ListValue { get; set; }
    
    internal class Converter : JsonConverter<OpenAiCompletionStopInput>
    {
        public override OpenAiCompletionStopInput Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new OpenAiCompletionStopInput { StringValue = reader.GetString() };
            }
            else if (reader.TokenType == JsonTokenType.StartArray)
            {
                var list = JsonSerializer.Deserialize<List<string>>(ref reader, options);
                return new OpenAiCompletionStopInput { ListValue = list };
            }
            else
            {
                throw new JsonException();
            }
        }

        public override void Write(
            Utf8JsonWriter writer, 
            OpenAiCompletionStopInput value, 
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