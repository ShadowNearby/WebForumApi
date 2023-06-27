using Ardalis.Result;
using MediatR;

namespace WebForumApi.Application.Features.Users.CreateUser;

public record CreateUserRequest : IRequest<Result>
{
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;

    public string Password { get; init; } = null!;
}