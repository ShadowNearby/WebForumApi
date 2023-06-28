using System;
using System.Collections.Generic;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Domain.Entities;

public class Answer : Entity<Guid>
{
    public override Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public Guid QuestionId { get; set; }
    public User CreateUser { get; set; } = null!;
    public UserId CreateUserId { get; set; }
    public string CreateUserUsername { get; set; } = null!;
    public string? CreateUserAvatar { get; set; }

    public DateTime CreateTime { get; set; } = DateTime.Now;
    public List<UserAnswerAction> UserAnswerActions { get; set; } = new();

    public long LikeCount { get; set; }
    public long DislikeCount { get; set; }
    public long StarCount { get; set; }
}