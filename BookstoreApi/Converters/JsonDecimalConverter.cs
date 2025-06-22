using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookstoreApi.Converters;

public class JsonDecimalConverter : JsonConverter<decimal>
{
    
        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("F2"));
        }

        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return  reader.GetDecimal();
        }
}
