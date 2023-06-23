using System;

namespace WebForumApi.Application.Features.Auth.Authenticate;

public record Jwt
{
    public string AccessToken { get; init; } = null!;
    public DateTime Expire { get; init; }
}