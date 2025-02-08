using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    /*
     * ref Utf8JsonReader reader: A reference to the reader that provides the JSON data. 
     *                            The reader maintains its position as it reads.
     * Type typeToConvert: The type that is being deserialized (in this case, DateOnly).
     * JsonSerializerOptions options: Options that can control the serialization behavior.
     */
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string dateString = reader.GetString()!;
        return DateOnly.Parse(dateString);
    }

    /*
     * Utf8JsonWriter writer: The writer that is used to write JSON data.
     * DateOnly value: The DateOnly instance that is being serialized.
     * JsonSerializerOptions options: Options that can control the serialization behavior.
     */
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }
}
