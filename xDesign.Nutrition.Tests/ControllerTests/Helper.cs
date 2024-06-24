using System.Text.Json;
using System.Text.Json.Serialization;
using xDesign.Nutrition.Api.Model;

namespace xDesign.Nutrition.Tests.ControllerTests;

public static class Helper
{
    private static JsonSerializerOptions SerializerOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerOptions.Default)
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        options.Converters.Add(new DoubleToIntConverter());
        
        return options;
    }

    public static async Task<IEnumerable<Food>> DeserialiseResponse(HttpResponseMessage response)
    {
        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<Food>>(responseJson, SerializerOptions())!;
    }

    public static IEnumerable<Food> ReadJsonFile(string path)
    {
        var json = File.ReadAllText($"./Resources/{path}");
        return JsonSerializer.Deserialize<IEnumerable<Food>>(json, SerializerOptions())!;
    }
}

public class DoubleToIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TryGetInt32(out var int32Value))
        {
            return int32Value;
        }

        if (reader.TryGetDouble(out var doubleValue))
        {
            return (int)doubleValue;
        }

        throw new JsonException($"The JSON value could not be converted to System.Int32.");
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}