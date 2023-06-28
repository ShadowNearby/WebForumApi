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
using BC=BCrypt.Net.BCrypt;

namespace WebForumApi.Application.Features.Auth.Authenticate;

public class AuthenticateHandler : IRequestHandler<AuthenticateRequest, Result<JwtDto>>
{
    private readonly IContext _context;
    private readonly ITokenService _tokenService;

    public AuthenticateHandler(IContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<Result<JwtDto>> Handle(
        AuthenticateRequest request,
        CancellationToken cancellationToken
    )
    {
        var user = await _context.Users
            .Select(
                x =>
                    new
                    {
                        x.Id,
                        x.Password,
                        x.Username,
                        x.Role,
                        x.LastLogin,
                        x.Avatar
                    }
            )
            .FirstOrDefaultAsync(x => x.Username.Equals(request.Username), cancellationToken);
        // update last login
        // var user = await _context.Users.FirstOrDefaultAsync(
        //     x => x.Username.Equals(request.Username), cancellationToken);
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

        if (!BC.Verify(request.Password, user.Password))
        {
            return Result.Invalid(
                new List<ValidationError>
                {
                    new()
                    {
                        Identifier = $"{nameof(request.Password)}", ErrorMessage = "Password is incorrect"
                    }
                }
            );
        }

        // user.LastLogin = DateTime.Now;

        JwtDto jwtDto = _tokenService.GenerateJwt(user.Username, user.Role, user.Id, user.Avatar);
        Token? token = await _context.Tokens.FirstOrDefaultAsync(
            x => x.UserId == user.Id,
            cancellationToken
        );
        token ??= new Token
        {
            UserId = user.Id
        };
        token.RefreshToken = jwtDto.RefreshToken;
        token.Expire = DateTime.Now.AddDays(1);
        await _context.SaveChangesAsync(cancellationToken);
        return jwtDto;
    }
}