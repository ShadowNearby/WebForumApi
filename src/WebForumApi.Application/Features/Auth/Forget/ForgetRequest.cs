using Ardalis.Result;
using MediatR;

namespace WebForumApi.Application.Features.Auth.Forget;

public class ForgetRequest : IRequest<Result>
{
    public string Username { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}