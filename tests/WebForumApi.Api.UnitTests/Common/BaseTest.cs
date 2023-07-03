using FluentAssertions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebForumApi.Api.UnitTests.Helpers;
using WebForumApi.Application.Features.Auth;
using WebForumApi.Application.Features.Auth.Authenticate;
using Xunit;

namespace WebForumApi.Api.UnitTests.Common;

[Collection("Test collection")]
public abstract class BaseTest : IAsyncLifetime
{
    protected static string? AdminToken;

    protected static string? UserToken;

    protected BaseTest(CustomWebApplicationFactory apiFactory)
    {
        App = apiFactory;

        Client = App.Client;
    }

    private CustomWebApplicationFactory App { get; }
    protected HttpClient Client { get; }

    public virtual async Task InitializeAsync()
    {
        await TestingDatabase.SeedDatabase(App.CreateContext);
        AdminToken ??= await GetAdminToken();
        UserToken ??= await GetUserToken();
    }

    public async Task DisposeAsync()
    {
        await App.ResetDatabaseAsync();
    }

    [Fact]
    public async Task<string> GetAdminToken()
    {
        // Act
        AuthenticateRequest loginData =
            new() { Username = "admin", Password = "admin" };

        JwtDto? response = await PostAsync<JwtDto>(address: "/api/auth/login", loginData);
        response.Should().NotBeNull();
        response!.Expire.Should().NotBe(DateTime.MinValue);
        response.AccessToken.Should().NotBeNullOrWhiteSpace();

        return response.AccessToken;
    }

    [Fact]
    public async Task<string> GetUserToken()
    {
        // Act
        AuthenticateRequest loginData =
            new() { Username = "user", Password = "user" };

        JwtDto? response = await PostAsync<JwtDto>(address: "/api/auth/login", loginData);
        response.Should().NotBeNull();
        response!.Expire.Should().NotBe(DateTime.MinValue);
        response.AccessToken.Should().NotBeNullOrWhiteSpace();

        return response.AccessToken;
    }

    protected void LoginAsAdmin()
    {
        UpdateBearerToken(AdminToken);
    }

    protected void LoginAsUser()
    {
        UpdateBearerToken(UserToken);
    }

    protected void UpdateBearerToken(string? token)
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", token);
    }

    protected async Task<T?> GetAsync<T>(string address, object? query = null)
    {
        if (query != null)
        {
            address += $"?{query.ToQueryString()}";
        }

        return await Client.GetFromJsonAsync<T>(address);
    }

    protected async Task<HttpResponseMessage> GetAsync(string address, object? query = null)
    {
        if (query != null)
        {
            address += $"?{query.ToQueryString()}";
        }

        return await Client.GetAsync(address);
    }

    protected async Task<T?> PostAsync<T>(string address, object? data = null)
    {
        HttpResponseMessage? response = await RequestAsync(address, data, Client.PostAsync);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    protected async Task<HttpResponseMessage> PostAsync(
        string address,
        object? data = null,
        bool ensureSuccess = false
    )
    {
        HttpResponseMessage? response = await RequestAsync(address, data, Client.PostAsync);
        if (ensureSuccess)
        {
            response.EnsureSuccessStatusCode();
        }

        return response;
    }

    protected async Task<T?> PutAsync<T>(string address, object? data = null)
    {
        HttpResponseMessage? response = await RequestAsync(address, data, Client.PutAsync);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    protected async Task<HttpResponseMessage> PutAsync(
        string address,
        object? data = null,
        bool ensureSuccess = false
    )
    {
        HttpResponseMessage? response = await RequestAsync(address, data, Client.PutAsync);
        if (ensureSuccess)
        {
            response.EnsureSuccessStatusCode();
        }

        return response;
    }

    protected async Task<T?> PatchAsync<T>(string address, object? data = null)
    {
        HttpResponseMessage? response = await RequestAsync(address, data, Client.PatchAsync);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    protected async Task<HttpResponseMessage> PatchAsync(
        string address,
        object? data = null,
        bool ensureSuccess = false
    )
    {
        HttpResponseMessage? response = await RequestAsync(address, data, Client.PatchAsync);
        if (ensureSuccess)
        {
            response.EnsureSuccessStatusCode();
        }

        return response;
    }

    protected async Task<HttpResponseMessage> DeleteAsync(
        string address,
        bool ensureSuccess = false
    )
    {
        HttpResponseMessage? response = await Client.DeleteAsync(address);
        if (ensureSuccess)
        {
            response.EnsureSuccessStatusCode();
        }

        return response;
    }

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
}