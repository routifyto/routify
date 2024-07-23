using System.Text.Json;
using System.Text.Json.Serialization;
using Routify.Core.Models;

namespace Routify.Core.Utils;

public class JsonObjectConverter : JsonConverter<JsonObject>
{
    public override JsonObject Read(
        ref Utf8JsonReader reader, 
        Type typeToConvert, 
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        var jsonObject = new JsonObject();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return jsonObject;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            var propertyName = reader.GetString();
            reader.Read();
            
            if (string.IsNullOrEmpty(propertyName))
                throw new JsonException();

            jsonObject[propertyName] = ReadValue(ref reader, options);
        }

        return jsonObject;
    }

    private object? ReadValue(
        ref Utf8JsonReader reader, 
        JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                return reader.GetString();
            case JsonTokenType.Number:
                if (reader.TryGetInt32(out var intValue))
                {
                    return intValue;
                }
                if (reader.TryGetInt64(out var longValue))
                {
                    return longValue;
                }
                return reader.GetDouble();
            case JsonTokenType.True:
                return true;
            case JsonTokenType.False:
                return false;
            case JsonTokenType.Null:
                return null;
            case JsonTokenType.StartObject:
                return Read(ref reader, typeof(JsonObject), options);
            case JsonTokenType.StartArray:
                var list = new List<object?>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndArray)
                    {
                        break;
                    }
                    list.Add(ReadValue(ref reader, options));
                }
                return list;
            default:
                throw new JsonException();
        }
    }

    public override void Write(
        Utf8JsonWriter writer, 
        JsonObject jsonObject, 
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (var kvp in jsonObject)
        {
            writer.WritePropertyName(kvp.Key);
            WriteValue(writer, kvp.Value, options);
        }

        writer.WriteEndObject();
    }

    private void WriteValue(
        Utf8JsonWriter writer, 
        object? value, 
        JsonSerializerOptions options)
    {
        switch (value)
        {
            case string s:
                writer.WriteStringValue(s);
                break;
            case int i:
                writer.WriteNumberValue(i);
                break;
            case long l:
                writer.WriteNumberValue(l);
                break;
            case double d:
                writer.WriteNumberValue(d);
                break;
            case bool b:
                writer.WriteBooleanValue(b);
                break;
            case null:
                writer.WriteNullValue();
                break;
            case JsonObject jsonObject:
                Write(writer, jsonObject, options);
                break;
            case List<object?> list:
                writer.WriteStartArray();
                foreach (var item in list)
                {
                    WriteValue(writer, item, options);
                }
                writer.WriteEndArray();
                break;
            default:
                throw new JsonException($"Unsupported value type: {value?.GetType()}");
        }
    }
}