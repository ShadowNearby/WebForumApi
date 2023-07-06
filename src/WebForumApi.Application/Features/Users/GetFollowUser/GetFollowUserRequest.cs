using Ardalis.Result;
using MediatR;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Users.GetFollowUser;

public record GetFollowUserRequest : PaginatedRequest, IRequest<Result<PaginatedList<UserDetailDto>>>
{
    public UserId UserId { get; init; }
}