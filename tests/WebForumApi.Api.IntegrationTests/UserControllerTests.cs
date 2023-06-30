using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using WebForumApi.Api.IntegrationTests.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Application.Features.Users.GetUsers;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Api.IntegrationTests;

public class UserControllerTests : BaseTest
{
    public UserControllerTests(CustomWebApplicationFactory apiFactory)
        : base(apiFactory)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        LoginAsAdmin();
    }


    #region GET

    [Fact]
    public async Task Get_AllUsers_ReturnsOk()
    {
        // Act
        PaginatedList<UserDetailDto>? response = await GetAsync<PaginatedList<UserDetailDto>>(
            "/api/User"
        );

        // Assert
        response.Should().NotBeNull();
        response!.Result.Should().OnlyHaveUniqueItems();
        response.Result.Should().HaveCount(2);
        response.CurrentPage.Should().Be(1);
        response.TotalItems.Should().Be(2);
        response.TotalPages.Should().Be(1);
    }

    [Fact]
    public async Task Get_AllUsersWithPaginationFilter_ReturnsOk()
    {
        // Act
        PaginatedList<UserDetailDto>? response = await GetAsync<PaginatedList<UserDetailDto>>(
            address: "/api/user",
            new GetUsersRequest
            {
                CurrentPage = 1, PageSize = 1
            }
        );

        // Assert
        response.Should().NotBeNull();
        response!.Result.Should().OnlyHaveUniqueItems();
        response.Result.Should().HaveCount(1);
        response.CurrentPage.Should().Be(1);
        response.TotalItems.Should().Be(2);
        response.TotalPages.Should().Be(2);
    }

    [Fact]
    public async Task Get_ExistingUsersWithFilter_ReturnsOk()
    {
        // Act
        PaginatedList<UserDetailDto>? response = await GetAsync<PaginatedList<UserDetailDto>>(
            address: "/api/user",
            new GetUsersRequest
            {
                Username = "admin"
            }
        );

        // Assert
        response.Should().NotBeNull();
        response!.Result.Should().OnlyHaveUniqueItems();
        response.Result.Should().HaveCount(1);
        response.CurrentPage.Should().Be(1);
        response.TotalItems.Should().Be(1);
        response.TotalPages.Should().Be(1);
    }

    [Fact]
    public async Task Get_NonExistingUsersWithFilter_ReturnsOk()
    {
        // Act
        PaginatedList<UserDetailDto>? response = await GetAsync<PaginatedList<UserDetailDto>>(
            address: "/api/user",
            new GetUsersRequest
            {
                Username = "admifsdfsdfsdjma"
            }
        );

        // Assert
        response.Should().NotBeNull();
        response!.Result.Should().BeEmpty();
        response.CurrentPage.Should().Be(1);
        response.TotalItems.Should().Be(0);
        response.TotalPages.Should().Be(0);
    }

    [Fact]
    public async Task GetById_ExistingUser_ReturnsOk()
    {
        // Act
        UserDetailDto? response = await GetAsync<UserDetailDto>(
            "/api/user/2e3b7a21-f06e-4c47-b28a-89bdaa3d2a37"
        );

        // Assert
        response.Should().NotBeNull();
        response!.Id.Should().NotBe(UserId.Empty);
    }

    [Fact]
    public async Task GetById_ExistingUser_ReturnsNotFound()
    {
        // Act
        HttpResponseMessage response = await GetAsync($"/api/user/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task GetQuestion_ReturnsNoContent()
    {
        // Act
        PaginatedList<QuestionCardDto>? response = await GetAsync<PaginatedList<QuestionCardDto>>($"/api/user/{Guid.NewGuid()}/questions");

        // Assert
        response.Should().NotBeNull();
        response!.Result.Count.Should().Be(0);
    }
    [Fact]
    public async Task GetQuestionLike_ReturnsNoContent()
    {
        // Act
        PaginatedList<QuestionCardDto>? response = await GetAsync<PaginatedList<QuestionCardDto>>($"/api/user/{Guid.NewGuid()}/questions/like");

        // Assert
        response.Should().NotBeNull();
        response!.Result.Count.Should().Be(0);
    }
    [Fact]
    public async Task GetQuestionStar_ReturnsNoContent()
    {
        // Act
        PaginatedList<QuestionCardDto>? response = await GetAsync<PaginatedList<QuestionCardDto>>($"/api/user/{Guid.NewGuid()}/questions/star");

        // Assert
        response.Should().NotBeNull();
        response!.Result.Count.Should().Be(0);
    }
    [Fact]
    public async Task GetAnswer_ReturnsNoContent()
    {
        // Act
        PaginatedList<AnswerCardDto>? response = await GetAsync<PaginatedList<AnswerCardDto>>($"/api/user/{Guid.NewGuid()}/answers");

        // Assert
        response.Should().NotBeNull();
        response!.Result.Count.Should().Be(0);
    }
    [Fact]
    public async Task GetAnswerLike_ReturnsNoContent()
    {
        // Act
        PaginatedList<AnswerCardDto>? response = await GetAsync<PaginatedList<AnswerCardDto>>($"/api/user/{Guid.NewGuid()}/answers/like");

        // Assert
        response.Should().NotBeNull();
        response!.Result.Count.Should().Be(0);
    }
    [Fact]
    public async Task GetAnswerStar_ReturnsNoContent()
    {
        // Act
        PaginatedList<AnswerCardDto>? response = await GetAsync<PaginatedList<AnswerCardDto>>($"/api/user/{Guid.NewGuid()}/answers/star");

        // Assert
        response.Should().NotBeNull();
        response!.Result.Count.Should().Be(0);
    }

    #endregion


    #region DELETE

    [Fact]
    public async Task Delete_ValidUser_ReturnsNoContent()
    {
        // Act
        HttpResponseMessage response = await DeleteAsync(
            "/api/user/2e3b7a21-f06e-4c47-b28a-89bdaa3d2a37"
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_InvalidUser_ReturnsNotFound()
    {
        // Act
        HttpResponseMessage response = await DeleteAsync($"/api/user/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_AsUserRole_ReturnsForbidden()
    {
        // Arrange
        LoginAsUser();

        // act
        HttpResponseMessage response = await DeleteAsync("/api/user/2e3b7a21-f06e-4c47-b28a-89bdaa3d2a37");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    #endregion
}