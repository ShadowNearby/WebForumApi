using System.Text.Json;

namespace WebForumApi.Application.Extensions.Serializer;

public class SystemSerializerService : ISerializerService
{
    public string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj);
    }
    public T? Deserialize<T>(string text)
    {
        return JsonSerializer.Deserialize<T>(text);
    }
}