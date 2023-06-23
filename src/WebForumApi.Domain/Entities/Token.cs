using System;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Domain.Entities;

public class Token : Entity<int>
{
    public override int Id { get; set; }
    public string RefreshToken { get; set; } = null!;
    public DateTime Expire { get; set; } = DateTime.Now;

    public User User { get; set; } = null!;
    public UserId UserId { get; set; }
}
