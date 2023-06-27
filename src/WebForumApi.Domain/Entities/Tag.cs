using System.Collections.Generic;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Domain.Entities;

public class Tag : Entity<long>
{
    public override long Id { get; set; }
    public string Content { get; set; } = null!;
    public string? Description { get; set; }

    public List<Question> Questions { get; set; } = new();
}