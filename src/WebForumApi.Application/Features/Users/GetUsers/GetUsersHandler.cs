using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Extensions;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Domain.Auth;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Users.GetUsers;

public class GetUsersHandler : IRequestHandler<GetUsersRequest, PaginatedList<UserDetailDto>>
{
    private readonly IContext _context;

    public GetUsersHandler(IContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<UserDetailDto>> Handle(
        GetUsersRequest request,
        CancellationToken cancellationToken
    )
    {
        IQueryable<User> users = _context.Users
            .WhereIf(
                !string.IsNullOrEmpty(request.Username),
                x => EF.Functions.Like(x.Username, $"%{request.Username}%")
            )
            .WhereIf(request.IsAdmin, x => x.Role == Roles.Admin);
        return await users
            .ProjectToType<UserDetailDto>()
            .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
    }
}