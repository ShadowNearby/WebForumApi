using System;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Domain.Auth.Interfaces;

public interface ISession
{
    public UserId UserId { get; }
    public string? Username { get; }
    public string? Avatar { get; }

    public DateTime Now { get; }
}