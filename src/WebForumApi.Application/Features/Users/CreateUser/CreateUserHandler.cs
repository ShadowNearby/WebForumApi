using Ardalis.Result;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities;
using BC = BCrypt.Net.BCrypt;

namespace WebForumApi.Application.Features.Users.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserRequest, Result>
{
    private const string DefaultAvatar =
        "https://pic2.zhimg.com/v2-eda9c6ea91435f99e850ba32743ef0fd_r.jpg";
    private readonly IContext _context;

    public CreateUserHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        User created = request.Adapt<User>();
        _context.Users.Add(created);
        created.Password = BC.HashPassword(request.Password);
        created.Avatar = DefaultAvatar;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
