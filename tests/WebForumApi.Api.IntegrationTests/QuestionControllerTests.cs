using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using WebForumApi.Api.IntegrationTests.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Users.Dto;

namespace WebForumApi.Api.IntegrationTests;

public class QuestionControllerTests : BaseTest
{
    public QuestionControllerTests(CustomWebApplicationFactory apiFactory) : base(apiFactory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        LoginAsAdmin();
    }

    private const string QuestionId = "2e3b7a21-f06e-4c47-b28a-89bdaa3d2a36";

    #region GET

    [Fact]
    public async Task Get_ExistQuestionById_ReturnOk()
    {
        const string questionId = "2e3b7a21-f06e-4c47-b28a-89bdaa3d2a36";
        const string answerId = "";
        QuestionDto? question = await GetAsync<QuestionDto>($"/api/question/{questionId}");
        question.Should().NotBeNull();
        question!.UserCard.Should().Be(new UserCardDto
        {
            Id = "2e3b7a21-f06e-4c47-b28a-89bdaa3d2a37", UserName = "admin", Avatar = ""
        });
        question.LikeCount.Should().Be(1);
        question.DislikeCount.Should().Be(0);
        question.StarCount.Should().Be(1);
        question.UserAction.Should().Be(new UserActionDto { UserLike = true, UserStar = true, UserDislike = false });
        question.Tags.Should().Equal(new List<TagDto> { new() { Id = 1, Content = "content", Description = "" } });
    }


    [Fact]
    public async Task Get_SearchQuestion()
    {
        PaginatedList<QuestionDto>? response =
            await GetAsync<PaginatedList<QuestionDto>>("/api/Question/search?tab=newest");

        Assert.NotNull(response);
        Assert.NotNull(response.Result);
        Assert.NotEmpty(response.Result);
    }

    [Fact]
    public async Task Get_QuestionAnswer()
    {
        PaginatedList<AnswerDto>? ans =
            await GetAsync<PaginatedList<AnswerDto>>($"/api/Question/{QuestionId}/answers");
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
        HttpResponseMessage response = await PostAsync($"/api/Question/{QuestionId}/like");

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_dislikeQuestion()
    {
        LoginAsUser();
        HttpResponseMessage response = await PostAsync($"/api/Question/{QuestionId}/dislike");

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Post_starQuestion()
    {
        LoginAsUser();
        HttpResponseMessage response = await PostAsync($"/api/Question/{QuestionId}/star");

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion
}