using Ardalis.Result;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using WebForumApi.Api.IntegrationTests.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Api.IntegrationTests;

public class AnswerTests : BaseTest
{
    public AnswerTests(CustomWebApplicationFactory apiFactory) : base(apiFactory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        LoginAsUser();
    }


    [Fact]
    public async Task Post_AddNewAnswer_ReturnCreate()
    {
        // Arrange
        LoginAsUser();

        // Act
        string questionId = await GetQuestionId();

        HttpResponseMessage response = await PostAsync("/api/Answer/add",
            new { QuestionId = questionId, Content = "Test content" });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_AddNewAnswer_ReturnBadRequest()
    {
        // Arrange
        LoginAsUser();

        // Act
        HttpResponseMessage response = await PostAsync("/api/answer/add",
            new { QuestionId = "08db7942-6ce1" });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_LikeAnswer_ReturnCreate()
    {
        // Arrange
        LoginAsUser();

        // Act
        string answerId = await GetAnswerId();

        // Assert
        HttpResponseMessage response = await PostAsync($"/api/answer/{answerId}/like");
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_LikeAnswer_ReturnNotFound()
    {
        // Arrange
        LoginAsUser();

        // Act
        HttpResponseMessage response = await PostAsync($"/api/answer/08db7942-6ce1/like");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_DislikeAnswer_ReturnCreate()
    {
        // Arrange
        LoginAsUser();

        // Act
        string answerId = await GetAnswerId();

        // Assert
        HttpResponseMessage response = await PostAsync($"/api/answer/{answerId}/dislike");
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_DislikeAnswer_ReturnNotFound()
    {
        // Arrange
        LoginAsUser();

        // Act
        HttpResponseMessage response = await PostAsync($"/api/answer/08db7942-6ce1/dislike");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Post_StarAnswer_ReturnCreate()
    {
        LoginAsUser();

        // Act
        string answerId = await GetAnswerId();

        // Assert
        HttpResponseMessage response = await PostAsync($"/api/answer/{answerId}/star");
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_StarAnswer_ReturnNotFound()
    {
        // Arrange
        LoginAsUser();

        // Act
        HttpResponseMessage response = await PostAsync($"/api/answer/08db7942-6ce1/star");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    #region Utils

    private async Task Logout()
    {
        HttpResponseMessage _ = await PostAsync("/api/auth/forget");
    }

    private async Task<string> GetQuestionId()
    {
        HttpResponseMessage _ = await PostAsync("/api/Question/add",
            new { Title = "Test title", Content = "Test content" });
        PaginatedList<QuestionDto>? res = await GetAsync<PaginatedList<QuestionDto>>("/api/Question/search?tab=newest");
        // Check
        Assert.NotNull(res);
        QuestionDto? question = res.Result[0];
        return question.Id;
    }

    private async Task<string> GetAnswerId()
    {
        string questionId = await GetQuestionId();
        HttpResponseMessage _ = await PostAsync("/api/Answer/add",
            new { QuestionId = questionId, Content = "Test content" });
        // search for answers
        PaginatedList<AnswerDto>? res = await GetAsync<PaginatedList<AnswerDto>>($"/api/question/{questionId}/answers");
        // Check
        Assert.NotNull(res);
        AnswerDto? answer = res.Result[0];
        return answer.Id;
    }

    #endregion
}