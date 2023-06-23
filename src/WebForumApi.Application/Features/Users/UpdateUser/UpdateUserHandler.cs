using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities;
using BC = BCrypt.Net.BCrypt;

namespace WebForumApi.Application.Features.Users.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, Result>
{
    private readonly IContext _context;

    public UpdateUserHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        // Guaranteed to be valid, because it comes from the session.
        User originalUser = await _context.Users.FirstAsync(
            x => x.Id == request.Id,
            cancellationToken
        );
        originalUser.Password =
            request.Password != null ? BC.HashPassword(request.Password) : originalUser.Password;
        originalUser.Username = request.Username ?? originalUser.Username;
        originalUser.Email = request.Email ?? originalUser.Email;
        originalUser.Location = request.Location ?? originalUser.Location;
        originalUser.About = request.About ?? originalUser.About;
        originalUser.Avatar = request.Avatar ?? originalUser.Avatar;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
