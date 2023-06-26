using System;

namespace WebForumApi.Application.Features.Auth;

public record JwtDto
{
    public string AccessToken { get; init; } = null!;

    public string RefreshToken { get; init; } = null!;
    public DateTime Expire { get; init; }
}