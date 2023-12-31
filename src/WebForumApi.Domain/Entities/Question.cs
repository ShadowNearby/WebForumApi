using System;
using System.Collections.Generic;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Domain.Entities;

public class Question : Entity<Guid>
{
    public override Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public User CreateUser { get; set; } = null!;
    public UserId CreateUserId { get; set; }
    public string CreateUserUsername { get; set; } = null!;
    public string CreateUserAvatar { get; set; } = null!;

    public DateTime CreateTime { get; set; } = DateTime.Now;

    public List<QuestionTag> QuestionTags { get; set; } = new();
    public List<Answer> Answers { get; set; } = new();
    public Guid? AcceptAnswerId { get; set; }
    public long AnswerCount { get; set; }
    public List<UserQuestionAction> UserQuestionActions { get; set; } = new();
    public long LikeCount { get; set; }
    public long DislikeCount { get; set; }
    public long StarCount { get; set; }
}