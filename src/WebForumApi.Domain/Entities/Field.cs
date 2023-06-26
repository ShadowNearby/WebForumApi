using System.Collections.Generic;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Domain.Entities;

public class Field : Entity<long>
{
    public override long Id { get; set; }
    public string Content { get; set; } = null!;
    public List<User> Users { get; set; } = new();
}