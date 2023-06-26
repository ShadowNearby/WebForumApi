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

    public List<User> LikeUsers { get; set; } = new();
    public List<User> DislikeUsers { get; set; } = new();
    public List<User> StarUsers { get; set; } = new();
}