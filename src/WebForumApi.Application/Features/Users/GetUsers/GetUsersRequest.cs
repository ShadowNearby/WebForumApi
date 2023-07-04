using Ardalis.Result;
using MediatR;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Users.Dto;

namespace WebForumApi.Application.Features.Users.GetUsers;

public record GetUsersRequest : PaginatedRequest, IRequest<Result<PaginatedList<UserDetailDto>>>
{
    public string? Username { get; init; }
    public string? Tab { get; init; }
}