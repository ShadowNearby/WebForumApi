using System.Collections.Generic;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Domain.Entities;

public class Tag : Entity<long>
{
    public override long Id { get; set; }
    public string Content { get; private set; } = null!;

    public List<Question> Questions { get; set; } = new();
}