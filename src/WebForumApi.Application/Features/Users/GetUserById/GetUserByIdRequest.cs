using Ardalis.Result;
using MediatR;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Users.GetUserById;

public record GetUserByIdRequest : IRequest<Result<UserDetailDto>>
{
    public UserId Id { get; init; }
}