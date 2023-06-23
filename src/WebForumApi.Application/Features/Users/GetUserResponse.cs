using System;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Users;

public record GetUserResponse
{
    public UserId Id { get; init; }

    public string Email { get; init; } = null!;

    public string Username { get; init; } = null!;

    public DateTime LastLogin { get; set; }

    public string? Location { get; set; }

    public string? Profile { get; set; }

    public string Avatar { get; set; } = null!;
    public bool IsAdmin { get; init; }
}