using Ardalis.Result;
using MediatR;

namespace WebForumApi.Application.Features.Auth.Authenticate;

public record AuthenticateRequest : IRequest<Result<Jwt>>
{
    public string Username { get; init; } = null!;

    public string Password { get; init; } = null!;
}