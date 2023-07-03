using FluentAssertions;
using System.Net;
using System.Net.Http;
using WebForumApi.Api.IntegrationTests.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Api.IntegrationTests;

public class QuestionTests : BaseTest
{
    public QuestionTests(CustomWebApplicationFactory apiFactory) : base(apiFactory)
    {
    }


    #region GET

    [Fact]
    public async Task Get_SearchQuestion()
    {
        HttpResponseMessage _ = await PostAsync("/api/Question/add",
            new { Title = "Test title", Content = "Test content" });
        PaginatedList<QuestionDto>? response =
            await GetAsync<PaginatedList<QuestionDto>>("/api/Question/search?tab=newest");

        Assert.NotNull(response);
        Assert.NotNull(response.Result);
    }

    [Fact]
    public async Task Get_QuestionAnswer()
    {
        PaginatedList<AnswerDto>? ans =
            await GetAsync<PaginatedList<AnswerDto>>("/api/Question/2e3b7a21-f06e-4c47-b28a-89bdaa3d2a36/answers");
        // Check
        Assert.NotNull(ans);
        Assert.NotNull(ans.Result);
        Assert.NotEmpty(ans.Result);
    }

    [Fact]
    public async Task Get_TaggedQuestions()
    {
        PaginatedList<QuestionDto>? res =
            await GetAsync<PaginatedList<QuestionDto>>("/api/Question/tagged/content?tab=newest");
        // Check
        Assert.NotNull(res);
        Assert.NotNull(res.Result);
        Assert.NotEmpty(res.Result);
    }

    #endregion

    #region POST

    [Fact]
    public async Task Post_AddNewQuestion()
    {
        LoginAsUser();
        HttpResponseMessage response = await PostAsync("/api/Question/add",
            new { Title = "Test title", Content = "Test content" });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_likeQuestion()
    {
        LoginAsUser();
        string questionId = await GetQuestionId();
        HttpResponseMessage response = await PostAsync($"/api/Question/{questionId}/like");

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_dislikeQuestion()
    {
        LoginAsUser();
        string questionId = await GetQuestionId();
        HttpResponseMessage response = await PostAsync($"/api/Question/{questionId}/dislike");

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_starQuestion()
    {
        LoginAsUser();
        string questionId = await GetQuestionId();
        HttpResponseMessage response = await PostAsync($"/api/Question/{questionId}/star");

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion


    #region Utils

    private async Task<string> GetQuestionId()
    {
        HttpResponseMessage _ = await PostAsync("/api/Question/add",
            new { Title = "Test title", Content = "Test content" });
        PaginatedList<QuestionDto>? res = await GetAsync<PaginatedList<QuestionDto>>("/api/Question/search?tab=newest");
        // Check

        Assert.NotNull(res);
        QuestionDto? item = res.Result.Find(r => r.Title == "Test title");
        Assert.NotNull(item);
        return item.Id;
    }

    #endregion
}