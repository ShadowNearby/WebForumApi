using MediatR;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;

namespace WebForumApi.Application.Features.Users.GetUsers;

public record GetUsersRequest : PaginatedRequest, IRequest<PaginatedList<GetUserResponse>>
{
    public string? Username { get; init; }
    public bool IsAdmin { get; init; }
}
