using System.Text.Json;

namespace Application.Common.Extensions;

public static class JsonSerializerExtensions
{
    public static T DeserializeNotNull<T>(this Stream stream, JsonSerializerOptions options) where T : class
    {
        var result = JsonSerializer.Deserialize<T>(stream, options);

        if (result is null)
        {
            throw new NullReferenceException(nameof(DeserializeNotNull));
        }

        return result;
    }
}