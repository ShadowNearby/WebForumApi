using FluentAssertions;
using System.Collections.Generic;
using WebForumApi.Api.IntegrationTests.Common;
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
        question.UserAction.Should().Be(new UserActionDto
        {
            UserLike = true, UserStar = true, UserDislike = false
        });
        question.Tags.Should().Equal(new List<TagDto>
        {
            new()
            {
                Id = 1, Content = "content", Description = ""
            }
        });
    }
}