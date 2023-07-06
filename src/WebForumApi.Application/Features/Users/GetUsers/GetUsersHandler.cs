using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Extensions;
using WebForumApi.Application.Features.Tags.Dto;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Users.GetUsers;

public class GetUsersHandler : IRequestHandler<GetUsersRequest, Result<PaginatedList<UserDetailDto>>>
{
    private readonly IContext _context;

    public GetUsersHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedList<UserDetailDto>>> Handle(
        GetUsersRequest request,
        CancellationToken cancellationToken
    )
    {
        IQueryable<User> users = _context.Users
            .WhereIf(
                !string.IsNullOrEmpty(request.Username),
                x => EF.Functions.Like(x.Username, $"%{request.Username}%")
            );
        IQueryable<UserDetailDto> unsorted = users
            .Select(u => new UserDetailDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Location = u.Location,
                About = u.About,
                Avatar = u.Avatar,
                FollowedCount = u.FollowedCount,
                FollowingCount = u.FollowingCount,
                RegisterTime = u.RegisterTime,
                Tags = u.Fields.Select(f => new TagDto
                {
                    Id = f.Id, Content = f.Content
                }).ToList()
            });

        return request.Tab switch
        {
            "newest" => await unsorted.OrderByDescending(x => x.RegisterTime).ToPaginatedListAsync(request.CurrentPage, request.PageSize),
            "heat" => await unsorted.OrderByDescending(x => x.FollowedCount).ToPaginatedListAsync(request.CurrentPage, request.PageSize),
            _ => Result.Invalid(new List<ValidationError>
            {
                new()
                {
                    Identifier = $"{nameof(request.Tab)}"
                }
            })
        };
    }
}