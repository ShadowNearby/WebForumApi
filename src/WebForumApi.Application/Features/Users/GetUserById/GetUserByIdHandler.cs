using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Users.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, Result<UserDetailDto>>
{
    private readonly IContext _context;

    public GetUserByIdHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<UserDetailDto>> Handle(
        GetUserByIdRequest request,
        CancellationToken cancellationToken
    )
    {
        User? result = await _context.Users.FirstOrDefaultAsync(
            x => x.Id == request.Id,
            cancellationToken
        );
        if (result is null)
        {
            return Result.NotFound();
        }

        return result.Adapt<UserDetailDto>();
    }
}