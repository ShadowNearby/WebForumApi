using Ardalis.Result;
using MediatR;
using System.Text.Json.Serialization;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Users.UpdateUser;

public record UpdateUserRequest : IRequest<Result>
{
    [JsonIgnore] public UserId Id { get; init; }

    public string? Password { get; init; }
    public string? Username { get; init; }
    public string? Email { get; init; }
    public string? Location { get; init; }
    public string? About { get; init; }
    public string? Avatar { get; init; }
}