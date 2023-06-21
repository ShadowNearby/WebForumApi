using Ardalis.Result;
using WebForumApi.Domain.Entities;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using BC = BCrypt.Net.BCrypt;

namespace WebForumApi.Application.Features.Users.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserRequest, Result<GetUserResponse>>
{
    private readonly IContext _context;
    
    
    public CreateUserHandler( IContext context)
    {
        _context = context;
    }
    public async Task<Result<GetUserResponse>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var created = request.Adapt<User>();
        _context.Users.Add(created);
        created.Password = BC.HashPassword(request.Password);
        await _context.SaveChangesAsync(cancellationToken);
        return created.Adapt<GetUserResponse>();
    }
}