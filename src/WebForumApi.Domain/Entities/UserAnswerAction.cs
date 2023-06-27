using System;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Domain.Entities;

public class UserAnswerAction
{
    public UserId UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid AnswerId { get; set; }
    public Answer Answer { get; set; } = null!;
    public bool IsLike { get; set; }
    public bool IsDislike { get; set; }
    public bool IsStar { get; set; }
}