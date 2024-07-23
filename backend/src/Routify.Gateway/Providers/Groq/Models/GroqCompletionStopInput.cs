using System.Text.Json;
using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Groq.Models;

[JsonConverter(typeof(Converter))]
internal record GroqCompletionStopInput
{
    public string? StringValue { get; set; }
    public List<string>? ListValue { get; set; }
    
    internal class Converter : JsonConverter<GroqCompletionStopInput>
    {
        public override GroqCompletionStopInput Read(
            ref Utf8JsonReader reader, 
            Type typeToConvert, 
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new GroqCompletionStopInput { StringValue = reader.GetString() };
            }

            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var list = JsonSerializer.Deserialize<List<string>>(ref reader, options);
                return new GroqCompletionStopInput { ListValue = list };
            }

            throw new JsonException();
        }

        public override void Write(
            Utf8JsonWriter writer, 
            GroqCompletionStopInput value, 
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