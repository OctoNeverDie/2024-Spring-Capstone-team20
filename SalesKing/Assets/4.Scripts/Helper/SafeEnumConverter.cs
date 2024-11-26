using Newtonsoft.Json;
using System;

public class SafeEnumConverter<T> : JsonConverter where T : struct
{
    private readonly T _defaultValue;

    public SafeEnumConverter(T defaultValue)
    { 
        _defaultValue = defaultValue;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(T);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        { 
            string enumText = reader.Value.ToString();

            if (Enum.TryParse<T>(enumText, true, out var enumValue))
            {
                return enumValue;
            }
            else 
            {
                return _defaultValue;
            }
        }

        return _defaultValue;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }
}
