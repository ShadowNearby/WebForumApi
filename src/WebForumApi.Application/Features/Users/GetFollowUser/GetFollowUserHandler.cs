using Ardalis.Result;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Tags.Dto;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Domain.Auth.Interfaces;

namespace WebForumApi.Application.Features.Users.GetFollowUser;

public class GetFollowUserHandler : IRequestHandler<GetFollowUserRequest, Result<PaginatedList<UserDetailDto>>>
{
    private readonly IContext _context;
    private readonly ISession _session;

    public GetFollowUserHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }
    public async Task<Result<PaginatedList<UserDetailDto>>> Handle(GetFollowUserRequest request, CancellationToken cancellationToken)
    {
        return await _context.UserFollows.Where(u => u.UserId == request.UserId).Select(u => new UserDetailDto
        {
            Id = u.UserFollowing.Id,
            Username = u.UserFollowing.Username,
            Email = u.UserFollowing.Email,
            Location = u.UserFollowing.Location,
            About = u.UserFollowing.About,
            Avatar = u.UserFollowing.Avatar,
            FollowedCount = u.UserFollowing.FollowedCount,
            FollowingCount = u.UserFollowing.FollowingCount,
            RegisterTime = u.UserFollowing.RegisterTime,
            Tags = u.UserFollowing.Fields.Select(f => new TagDto
            {
                Id = f.Id, Content = f.Content
            }).ToList()
        }).OrderByDescending(x => x.RegisterTime).ToPaginatedListAsync(request.CurrentPage, request.PageSize);
    }
}