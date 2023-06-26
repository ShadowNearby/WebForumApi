using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Auth.Refresh;

public class RefreshHandler : IRequestHandler<RefreshRequest, Result<JwtDto>>
{
    private readonly IContext _context;
    private readonly ITokenService _tokenService;

    public RefreshHandler(ITokenService tokenService, IContext context)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<Result<JwtDto>> Handle(
        RefreshRequest request,
        CancellationToken cancellationToken
    )
    {
        Token? token = await _context.Tokens.FirstOrDefaultAsync(
            t => t.RefreshToken.Equals(request.RefreshToken),
            cancellationToken
        );
        if (token == null)
        {
            return Result.Invalid(
                new List<ValidationError>
                {
                    new()
                    {
                        Identifier = $"{nameof(request.RefreshToken)}", ErrorMessage = "RefreshToken is incorrect"
                    }
                }
            );
        }

        if (token.Expire.CompareTo(DateTime.Now) < 0)
        {
            return Result.Invalid(
                new List<ValidationError>
                {
                    new()
                    {
                        Identifier = $"{nameof(request.RefreshToken)}", ErrorMessage = "RefreshToken is expired"
                    }
                }
            );
        }

        var user = await _context.Users
            .Select(
                x =>
                    new
                    {
                        x.Id, x.Username, x.Role
                    }
            )
            .FirstAsync(x => x.Id == token.UserId, cancellationToken);
        JwtDto jwtDto = _tokenService.GenerateJwt(user.Username, user.Role, user.Id);
        token.RefreshToken = jwtDto.RefreshToken;
        token.Expire = DateTime.Now.AddDays(1);
        await _context.SaveChangesAsync(cancellationToken);
        return jwtDto;
    }
}