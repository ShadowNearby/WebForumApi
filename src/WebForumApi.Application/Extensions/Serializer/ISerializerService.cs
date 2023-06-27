namespace WebForumApi.Application.Extensions.Serializer;

public interface ISerializerService
{
    string Serialize<T>(T obj);
    T? Deserialize<T>(string text);
}