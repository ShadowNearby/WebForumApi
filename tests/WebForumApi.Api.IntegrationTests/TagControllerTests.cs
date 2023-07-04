using FluentAssertions;
using System.Net;
using System.Net.Http;
using WebForumApi.Api.IntegrationTests.Common;

namespace WebForumApi.Api.IntegrationTests;

public class TagControllerTests : BaseTest
{
    public TagControllerTests(CustomWebApplicationFactory apiFactory) : base(apiFactory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        LoginAsAdmin();
    }

    #region GET

    [Fact]
    public async Task Get_GetAllTags_ReturnOk()
    {
        // Arrange
        LoginAsAdmin();

        // Act
        HttpResponseMessage response = await GetAsync("/api/tag/");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion

    #region POST

    [Fact]
    public async Task Post_AddNewTag_ReturnCreate()
    {
        // Arrange
        LoginAsAdmin();

        // Act
        HttpResponseMessage response = await PostAsync("/api/tag/add",
            new { Content = "Test content", Description = "Test description" });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_AddNewTag_ReturnBadRequest()
    {
        // Arrange
        LoginAsAdmin();

        // Act
        HttpResponseMessage response = await PostAsync("/api/tag/add",
            new { Content = "" });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_ModifyTag_ReturnOk()
    {
        // Arrange
        LoginAsAdmin();

        // Act
        HttpResponseMessage response = await PostAsync("/api/tag/modify",
            new { TagId = 1, Description = "Test description" });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion
}