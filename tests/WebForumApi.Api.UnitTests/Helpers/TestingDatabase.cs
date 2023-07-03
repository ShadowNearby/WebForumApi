using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities;
using BC = BCrypt.Net.BCrypt;

namespace WebForumApi.Api.UnitTests.Helpers;

public static class TestingDatabase
{
    private static readonly User[] GetSeedingUsers =
    {
        new()
        {
            Id = new Guid("2e3b7a21-f06e-4c47-b28a-89bdaa3d2a37"),
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

    private static readonly List<Question> GetSeedingQuestions = new();
    private static readonly List<Answer> GetSeedingAnswers = new();
    private static readonly List<Tag> GetSeedingTags = new();
    private static readonly List<Field> GetSeedingFields = new();
    private static readonly List<QuestionTag> GetSeedingQuestionTags = new();
    private static readonly List<UserQuestionAction> GetSeedingUserQuestionActions = new();
    private static readonly List<UserAnswerAction> GetSeedingUserAnswerActions = new();
    private static readonly List<UserFollow> GetSeedingUserFollows = new();

    public static async Task SeedDatabase(Func<IContext> contextFactory)
    {
        await using IContext? db = contextFactory();
        await db.Users.ExecuteDeleteAsync();
        db.Users.AddRange(GetSeedingUsers);
        db.Answers.AddRange(GetSeedingAnswers);
        db.Questions.AddRange(GetSeedingQuestions);
        db.Tags.AddRange(GetSeedingTags);
        db.Fields.AddRange(GetSeedingFields);
        db.UserFollows.AddRange(GetSeedingUserFollows);
        db.QuestionTags.AddRange(GetSeedingQuestionTags);
        db.UserAnswerActions.AddRange(GetSeedingUserAnswerActions);
        db.UserQuestionActions.AddRange(GetSeedingUserQuestionActions);
        await db.SaveChangesAsync();
    }
}