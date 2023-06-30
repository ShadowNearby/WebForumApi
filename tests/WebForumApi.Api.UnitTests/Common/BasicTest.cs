using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebForumApi.Api.UnitTests.Common;

public class BasicTest
{
    private HttpClient Client { get; } = new();
    private static async Task<HttpResponseMessage> RequestAsync(
        string address,
        object? data,
        Func<string, HttpContent, Task<HttpResponseMessage>> verb
    )
    {
        string? json = JsonSerializer.Serialize(data);

        HttpResponseMessage response;
        using StringContent? content = new(json, Encoding.UTF8, mediaType: "application/json");

        if (data is HttpContent httpContent)
        {
            response = await verb(address, httpContent);
        }
        else
        {
            response = await verb(address, content);
        }

        return response;
    }
    private async Task<T?> PostAsync<T>(string address, object? data = null)
    {
        HttpResponseMessage? response = await RequestAsync(address, data, Client.PostAsync);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }
}