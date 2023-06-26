using MassTransit;
using System;
using System.Collections.Generic;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Domain.Entities;

public class User : Entity<UserId>
{
    public override UserId Id { get; set; } = NewId.NextGuid();
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime RegisterTime { get; set; } = DateTime.Now;

    public DateTime LastLogin { get; set; }

    public string? Location { get; set; }
    public string? About { get; set; }
    public string Avatar { get; set; } = null!;
    public bool IsBanned { get; set; }
    public Token? Token { get; init; }

    public List<Question> LikeQuestions { get; set; } = new();
    public List<Question> DislikeQuestions { get; set; } = new();
    public List<Question> StarQuestions { get; set; } = new();
    public List<Answer> LikeAnswers { get; set; } = new();
    public List<Answer> DislikeAnswers { get; set; } = new();
    public List<Answer> StarAnswers { get; set; } = new();
    public List<Field> Fields { get; set; } = new();
    public List<Question> CreateQuestions { get; set; } = new();
    public List<Answer> CreateAnswers { get; set; } = new();
}