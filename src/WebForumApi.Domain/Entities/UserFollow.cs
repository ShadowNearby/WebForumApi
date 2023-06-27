using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Domain.Entities;

public class UserFollow
{
    public UserId UserId { get; set; }
    public User User { get; set; } = null!;
    public UserId UserIdFollowing { get; set; }
    public User UserFollowing { get; set; } = null!;
}