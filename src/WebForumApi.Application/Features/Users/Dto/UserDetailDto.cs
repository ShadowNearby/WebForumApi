using System;
using System.Collections.Generic;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Users.Dto;

public record UserDetailDto
{
    public UserId Id { get; init; }

    public string Email { get; init; } = null!;

    public string Username { get; init; } = null!;

    public DateTime LastLogin { get; init; }
    public DateTime RegisterTime { get; init; }

    public string? Location { get; init; }

    public string? About { get; init; }

    public string Avatar { get; init; } = null!;
    public long FollowedCount { get; init; }

    public List<TagDto> Tags { get; init; } = new();

    // public bool IsAdmin { get; init; }
}