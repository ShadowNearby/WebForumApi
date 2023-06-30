using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities;
using BC=BCrypt.Net.BCrypt;

namespace WebForumApi.Api.IntegrationTests.Helpers;

public static class TestingDatabase
{
    private const string AdminUserId = "2e3b7a21-f06e-4c47-b28a-89bdaa3d2a37";
    private const string QuestionId = "2e3b7a21-f06e-4c47-b28a-89bdaa3d2a36";
    private const string AnswerId = "2e3b7a21-f06e-4c47-b28a-89bdaa3d2a36";
    private static readonly User[] GetSeedingUsers =
    {
        new()
        {
            Id = new Guid(AdminUserId),
            Password = BC.HashPassword("admin"),
            Email = "admin@qq.com",
            Role = "Admin",
            Username = "admin",
            Avatar = ""
        },
        new()
        {
            Id = new Guid("c68acd7b-9054-4dc3-b536-17a1b81fa7a3"),
            Password = BC.HashPassword("user"),
            Email = "user@qq.com",
            Role = "User",
            Username = "user",
            Avatar = ""
        }
    };
    private static readonly User Admin = GetSeedingUsers[0];
    private static readonly User User = GetSeedingUsers[1];

    private static readonly List<Answer> GetSeedingAnswers = new()
    {
        new Answer
        {
            Id = new Guid(AnswerId),
            Content = "content",
            CreateUser = Admin,
            CreateUserId = Admin.Id,
            CreateUserAvatar = Admin.Avatar,
            CreateUserUsername = Admin.Username,
            LikeCount = 1,
            StarCount = 1
        }
    };
    private static readonly List<Question> GetSeedingQuestions = new()
    {
        new Question
        {
            Id = new Guid(QuestionId),
            Content = "content",
            Title = "title",
            CreateUserId = Admin.Id,
            CreateUserAvatar = Admin.Avatar,
            CreateUserUsername = Admin.Username,
            Answers = new List<Answer>
            {
                GetSeedingAnswers[0]
            },
            LikeCount = 1,
            StarCount = 1
        }
    };
    private static readonly List<Tag> GetSeedingTags = new()
    {
        new Tag
        {
            Id = 1, Content = "content", Description = ""
        }
    };
    private static readonly List<Field> GetSeedingFields = new()
    {
        new Field
        {
            Id = 1, Content = "content"
        }
    };
    private static readonly Answer Answer1 = GetSeedingAnswers[0];
    private static readonly Question Question1 = GetSeedingQuestions[0];
    private static readonly Tag Tag1 = GetSeedingTags[0];
    private static readonly List<QuestionTag> GetSeedingQuestionTags = new()
    {
        new QuestionTag
        {
            QuestionId = Question1.Id, TagId = Tag1.Id
        }
    };
    private static readonly List<UserQuestionAction> GetSeedingUserQuestionActions = new()
    {
        new UserQuestionAction
        {
            UserId = Admin.Id,
            QuestionId = Question1.Id,
            IsDislike = false,
            IsLike = true,
            IsStar = true
        }
    };
    private static readonly List<UserAnswerAction> GetSeedingUserAnswerActions = new()
    {
        new UserAnswerAction
        {
            UserId = Admin.Id,
            AnswerId = Answer1.Id,
            IsDislike = false,
            IsLike = true,
            IsStar = true
        }
    };
    private static readonly List<UserFollow> GetSeedingUserFollows = new()
    {
        new UserFollow
        {
            UserId = Admin.Id, UserIdFollowing = User.Id
        }
    };
    public static async Task SeedDatabase(Func<IContext> contextFactory)
    {
        await using IContext? db = contextFactory();
        await db.Users.ExecuteDeleteAsync();
        await db.Answers.ExecuteDeleteAsync();
        await db.Questions.ExecuteDeleteAsync();
        await db.Tags.ExecuteDeleteAsync();
        await db.Fields.ExecuteDeleteAsync();
        await db.UserFollows.ExecuteDeleteAsync();
        await db.QuestionTags.ExecuteDeleteAsync();
        await db.UserAnswerActions.ExecuteDeleteAsync();
        await db.UserQuestionActions.ExecuteDeleteAsync();
        await db.Users.AddRangeAsync(GetSeedingUsers);
        await db.Answers.AddRangeAsync(GetSeedingAnswers);
        await db.Questions.AddRangeAsync(GetSeedingQuestions);
        await db.Tags.AddRangeAsync(GetSeedingTags);
        await db.Fields.AddRangeAsync(GetSeedingFields);
        await db.UserFollows.AddRangeAsync(GetSeedingUserFollows);
        await db.QuestionTags.AddRangeAsync(GetSeedingQuestionTags);
        await db.UserAnswerActions.AddRangeAsync(GetSeedingUserAnswerActions);
        await db.UserQuestionActions.AddRangeAsync(GetSeedingUserQuestionActions);
        await db.SaveChangesAsync();
    }
}