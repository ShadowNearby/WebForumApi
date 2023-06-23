using MassTransit;
using System;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Domain.Entities;

public class User : Entity<UserId>
{
    public override UserId Id { get; set; } = NewId.NextGuid();
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime RegisterTime { get; set; } = DateTime.Now;

    public DateTime LastLogin { get; set; }

    public string? Location { get; set; }
    public string? Profile { get; set; }
    public string Avatar { get; set; } = null!;
    public bool IsBanned { get; set; } = false;
    public Token? Token { get; init; }
}