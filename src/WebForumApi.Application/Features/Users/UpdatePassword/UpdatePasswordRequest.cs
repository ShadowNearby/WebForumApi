using Ardalis.Result;
using WebForumApi.Domain.Entities.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace WebForumApi.Application.Features.Users.UpdatePassword;

public record UpdatePasswordRequest : IRequest<Result>
{
    [JsonIgnore]
    public UserId Id { get; init; }
    
    public string Password { get; init; } = null!;
}