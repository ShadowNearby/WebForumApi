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

    public List<Tag> Tags { get; set; } = new();
    public List<Answer> Answers { get; set; } = new();
    public List<User> LikeUsers { get; set; } = new();
    public List<User> DislikeUsers { get; set; } = new();
    public List<User> StarUsers { get; set; } = new();
}