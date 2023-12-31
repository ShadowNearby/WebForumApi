using Ardalis.Result;
using MediatR;
using WebForumApi.Application.Features.Auth.Authenticate;

namespace WebForumApi.Application.Features.Auth.Refresh;

public record RefreshRequest : IRequest<Result<JwtDto>>
{
    public string RefreshToken { get; init; } = null!;
}