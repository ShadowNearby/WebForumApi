using System;

namespace WebForumApi.Application.Cache;

public interface ISerializerService
{
    string Serialize<T>(T obj);
    T Deserialize<T>(string text);
}