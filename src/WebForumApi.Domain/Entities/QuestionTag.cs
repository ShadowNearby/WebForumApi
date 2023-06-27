using System;

namespace WebForumApi.Domain.Entities;

public class QuestionTag
{
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public long TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}