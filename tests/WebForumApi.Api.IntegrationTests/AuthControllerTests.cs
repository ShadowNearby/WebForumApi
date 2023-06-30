using Bogus;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using WebForumApi.Api.IntegrationTests.Common;
using WebForumApi.Application.Features.Auth;
using WebForumApi.Application.Features.Auth.Authenticate;
using WebForumApi.Application.Features.Auth.Forget;
using WebForumApi.Application.Features.Auth.Register;
using WebForumApi.Application.Features.Users.UpdateUser;

namespace WebForumApi.Api.IntegrationTests;

public class AuthControllerTests : BaseTest
{
    public AuthControllerTests(CustomWebApplicationFactory apiFactory) : base(apiFactory)
    {
    }
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        LoginAsAdmin();
    }

    #region PUT

    [Fact]
    public async Task Patch_ValidUser_UpdatePassword_NoContent()
    {
        // Arrange
        Faker<RegisterRequest> userFaker = new();

        RegisterRequest? newUser = userFaker.RuleFor(x => x.Username, f => f.Internet.UserName())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Password, f => f.Internet.Password())
            .Generate();
        HttpResponseMessage response = await PostAsync(address: "/api/auth/register", newUser);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        response = await PostAsync(address: "/api/auth/login", newUser);
        JwtDto? newUserToken = await response.Content.ReadFromJsonAsync<JwtDto>();
        UpdateBearerToken(newUserToken!.AccessToken);

        // Act
        response = await PutAsync(
            address: "/api/user/me/update",
            new UpdateUserRequest
            {
                Password = "mypasswordisverynice"
            }
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion

    #region Post

    [Fact]
    public async Task Authenticate_Forget_Password_Returns()
    {
        ForgetRequest request = new()
        {
            Username = "user", Password = "useruser", Email = "user@qq.com"
        };
        HttpResponseMessage response = await PostAsync(address: "/api/auth/forget", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Theory]
    [InlineData("admin@boilerplate.com", "incorrect")]
    [InlineData("admin@incorrect.com", "testpassword123")]
    public async Task Authenticate_IncorretUserOrPassword(string email, string password)
    {
        // Act
        AuthenticateRequest loginData = new()
        {
            Username = email, Password = password
        };
        HttpResponseMessage response = await PostAsync(address: "/api/auth/login", loginData);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_ValidUser_ReturnsCreated()
    {
        // Arrange
        Faker<RegisterRequest> userFaker = new();

        // Act
        RegisterRequest? newUser = userFaker.RuleFor(x => x.Username, f => f.Internet.UserName())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Password, f => f.Internet.Password())
            .Generate();
        HttpResponseMessage? response = await PostAsync(address: "/api/auth/register", newUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_EmaillessUser_ReturnsBadRequest()
    {
        // Act
        RegisterRequest newUser = new()
        {
            Password = "mypasswordisnice"
        };
        HttpResponseMessage response = await PostAsync(address: "/api/auth/register", newUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_PasswordlessUser_ReturnsBadRequest()
    {
        // Arrange
        Faker<RegisterRequest> userFaker = new();

        // Act
        RegisterRequest? newUser = userFaker.RuleFor(x => x.Username, f => f.Internet.UserName())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Password, _ => null!)
            .Generate();
        HttpResponseMessage response = await PostAsync(address: "/api/auth/register", newUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_EmptyUser_ReturnsBadRequest()
    {
        // Act
        RegisterRequest newUser = new();
        HttpResponseMessage response = await PostAsync(address: "/api/auth/register", newUser);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion
}