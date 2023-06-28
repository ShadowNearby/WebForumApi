using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Auth.Forget;

public class ForgetHandler : IRequestHandler<ForgetRequest, Result>
{
    private readonly IContext _context;

    public ForgetHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(ForgetRequest request, CancellationToken cancellationToken)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(
            x => request.Username.Equals(x.Username),
            cancellationToken
        );
        if (user == null)
        {
            return Result.Invalid(
                new List<ValidationError>
                {
                    new()
                    {
                        Identifier = $"{nameof(request.Username)}", ErrorMessage = "Username is incorrect"
                    }
                }
            );
        }

        if (user.Email != request.Email)
        {
            return Result.Invalid(
                new List<ValidationError>
                {
                    new()
                    {
                        Identifier = $"{nameof(request.Email)}", ErrorMessage = "Email is incorrect"
                    }
                }
            );
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}