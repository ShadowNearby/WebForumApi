using Ardalis.Result;
using WebForumApi.Application.Common.Responses;
using MediatR;

namespace WebForumApi.Application.Features.Auth.Authenticate;

public record AuthenticateRequest : IRequest<Result<Jwt>>
{
    public string Email { get; init; } = null!;

    public string Password { get; init; }  = null!;
}