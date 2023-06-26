using MediatR;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Users.Dto;

namespace WebForumApi.Application.Features.Users.GetUsers;

public record GetUsersRequest : PaginatedRequest, IRequest<PaginatedList<UserDetailDto>>
{
    public string? Username { get; init; }
    public bool IsAdmin { get; init; }
}